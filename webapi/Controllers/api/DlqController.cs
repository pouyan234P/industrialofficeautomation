using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace webapi.Controllers.api.admin
{
    /// <summary>
    /// Dead Letter Queue (DLQ) monitor and replay controller.
    ///
    /// Endpoints:
    ///   GET  /api/dlq/status          — count of messages in each DLQ
    ///   GET  /api/dlq/peek/{queue}    — read up to 10 messages without removing them
    ///   POST /api/dlq/replay/{queue}  — move all messages from DLQ back to the main queue
    ///   DELETE /api/dlq/purge/{queue} — discard all messages in a DLQ (use with care)
    ///
    /// When a RabbitMQ consumer fails a message 3 times it NACKs it to the DLQ.
    /// Use /peek to inspect the payload, fix the bug, deploy, then /replay to reprocess.
    /// </summary>
    [AllowAnonymous]   // TODO: lock down to Admin role before production
    [Route("api/[controller]")]
    [ApiController]
    public class DlqController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DlqController> _logger;

        // The three DLQs created by the consumers
        private static readonly string[] KnownDlqs =
        [
            "myreferral.dlq",
            "updatereferral.dlq",
            "myels.dlq"
        ];

        // Maps each DLQ back to its source queue for replay
        private static readonly Dictionary<string, string> DlqToSourceQueue = new()
        {
            { "myreferral.dlq",    "myreferral"    },
            { "updatereferral.dlq","updatereferral" },
            { "myels.dlq",         "myels"          }
        };

        public DlqController(IConfiguration configuration, ILogger<DlqController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Returns the message count in each DLQ.
        /// A non-zero count means messages are waiting to be inspected and replayed.
        /// </summary>
        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            using var connection = await CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var status = new Dictionary<string, object>();

            foreach (var dlq in KnownDlqs)
            {
                try
                {
                    // QueueDeclarePassive just reads metadata without creating/modifying the queue
                    var result = await channel.QueueDeclarePassiveAsync(dlq);
                    status[dlq] = new
                    {
                        messageCount = result.MessageCount,
                        consumerCount = result.ConsumerCount
                    };
                }
                catch
                {
                    // Queue doesn't exist yet (no failures have occurred)
                    status[dlq] = new { messageCount = 0, note = "queue not yet created" };
                }
            }

            return Ok(status);
        }

        /// <summary>
        /// Peeks at up to 10 messages in the given DLQ without removing them.
        /// Use this to inspect the payload before deciding to replay or purge.
        ///
        /// Example: GET /api/dlq/peek/myreferral.dlq
        /// </summary>
        [HttpGet("peek/{queue}")]
        public async Task<IActionResult> PeekDlq(string queue)
        {
            if (!KnownDlqs.Contains(queue))
                return BadRequest($"Unknown DLQ: {queue}. Valid queues: {string.Join(", ", KnownDlqs)}");

            using var connection = await CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var messages = new List<object>();
            const int maxPeek = 10;

            for (var i = 0; i < maxPeek; i++)
            {
                var result = await channel.BasicGetAsync(queue, autoAck: false);
                if (result == null) break;  // queue is empty

                var body = Encoding.UTF8.GetString(result.Body.ToArray());

                messages.Add(new
                {
                    deliveryTag   = result.DeliveryTag,
                    redelivered   = result.Redelivered,
                    correlationId = result.BasicProperties.CorrelationId,
                    timestamp     = result.BasicProperties.Timestamp.UnixTime,
                    body          = TryParseJson(body)
                });

                // NACK with requeue: true — put the message back so we don't lose it
                await channel.BasicNackAsync(result.DeliveryTag, multiple: false, requeue: true);
            }

            _logger.LogInformation("Peeked {Count} messages from DLQ {Queue}", messages.Count, queue);

            return Ok(new { queue, count = messages.Count, messages });
        }

        /// <summary>
        /// Moves ALL messages from the DLQ back to the original queue for reprocessing.
        ///
        /// Only do this after:
        ///   1. You've fixed the bug that caused the failures
        ///   2. You've deployed the fix
        ///   3. You've peeked the messages to confirm they look valid
        ///
        /// Example: POST /api/dlq/replay/myreferral.dlq
        /// </summary>
        [HttpPost("replay/{queue}")]
        public async Task<IActionResult> ReplayDlq(string queue)
        {
            if (!KnownDlqs.Contains(queue))
                return BadRequest($"Unknown DLQ: {queue}");

            if (!DlqToSourceQueue.TryGetValue(queue, out var sourceQueue))
                return BadRequest($"No source queue mapping found for {queue}");

            using var connection = await CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var replayed = 0;

            while (true)
            {
                var result = await channel.BasicGetAsync(queue, autoAck: false);
                if (result == null) break;

                var body = result.Body.ToArray();

                // Preserve the original CorrelationId so downstream logs stay traceable
                var props = new BasicProperties
                {
                    Persistent    = true,
                    CorrelationId = result.BasicProperties.CorrelationId
                };

                // Publish to the main queue
                await channel.BasicPublishAsync(
                    exchange:         "",
                    routingKey:       sourceQueue,
                    mandatory:        false,
                    basicProperties:  props,
                    body:             body);

                // ACK from DLQ only after successfully publishing to source queue
                await channel.BasicAckAsync(result.DeliveryTag, multiple: false);
                replayed++;
            }

            _logger.LogInformation(
                "Replayed {Count} messages from {DlqName} to {SourceQueue}",
                replayed, queue, sourceQueue);

            return Ok(new
            {
                replayed,
                from = queue,
                to   = sourceQueue,
                message = $"{replayed} messages moved back to {sourceQueue} for reprocessing"
            });
        }

        /// <summary>
        /// Permanently deletes all messages in the DLQ.
        /// Use only when you know the messages are invalid and should not be retried.
        ///
        /// Example: DELETE /api/dlq/purge/myels.dlq
        /// </summary>
        [HttpDelete("purge/{queue}")]
        public async Task<IActionResult> PurgeDlq(string queue)
        {
            if (!KnownDlqs.Contains(queue))
                return BadRequest($"Unknown DLQ: {queue}");

            using var connection = await CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var result = await channel.QueuePurgeAsync(queue);

            _logger.LogWarning("DLQ {Queue} purged. {Count} messages deleted.", queue, result);

            return Ok(new { purged = result, queue });
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private async Task<IConnection> CreateConnectionAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"]     ?? "rabbitmq",
                UserName = _configuration["RabbitMQ:Username"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest"
            };
            return await factory.CreateConnectionAsync();
        }

        private static object TryParseJson(string raw)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject(raw)!;
            }
            catch
            {
                return raw;  // return raw string if it's not valid JSON
            }
        }
    }
}
