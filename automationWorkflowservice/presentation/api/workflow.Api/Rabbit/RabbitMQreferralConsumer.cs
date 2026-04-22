using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog.Context;
using System.Text;
using workflow.Appliction.DTO;
using workflow.Appliction.Feature.referralFeature.request.Command;

namespace workflow.Api.Rabbit
{
    /// <summary>
    /// Consumes the "myreferral" queue and persists referrals to MongoDB via MediatR.
    ///
    /// Key improvements over the original:
    ///   1. Dead Letter Queue (DLQ) — messages that fail after all retries go to
    ///      "myreferral.dlq" instead of disappearing or looping forever
    ///   2. Per-message retry — up to 3 attempts with a short delay before NACKing
    ///      to the DLQ
    ///   3. Durable queue — messages survive RabbitMQ restarts
    ///   4. CorrelationId forwarded from BasicProperties into Serilog context
    ///   5. ILogger replaces Console.WriteLine throughout
    ///   6. RabbitMQ config read from IConfiguration, not hardcoded
    /// </summary>
    public class RabbitMQreferralConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;  // use scope per message for MediatR
        private readonly ILogger<RabbitMQreferralConsumer> _logger;
        private IConnection? _connection;
        private IChannel? _channel;

        private const int MaxRetries = 3;
        private const string QueueConfigKey = "TopicAndQueueNames:myreferral";

        public RabbitMQreferralConsumer(
            IConfiguration configuration,
            IServiceScopeFactory scopeFactory,
            ILogger<RabbitMQreferralConsumer> logger)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // ── Connect with retry loop ──────────────────────────────────────
            // Keep retrying until RabbitMQ is reachable (handles slow Docker starts)
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var factory = new ConnectionFactory
                    {
                        HostName = _configuration["RabbitMQ:Host"] ?? "192.168.1.173",
                        UserName = _configuration["RabbitMQ:Username"] ?? "guest",
                        Password = _configuration["RabbitMQ:Password"] ?? "guest",
                        AutomaticRecoveryEnabled = true,
                        NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
                    };

                    _connection = await factory.CreateConnectionAsync(stoppingToken);
                    _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                    // ── Dead Letter Queue setup ──────────────────────────────
                    // 1. Declare the DLQ first (it must exist before the main queue references it)
                    var queueName = _configuration.GetValue<string>(QueueConfigKey)!;
                    var dlqName = $"{queueName}.dlq";

                    await _channel.QueueDeclareAsync(
                        queue: dlqName,
                        durable: true,          // survive restarts
                        exclusive: false,
                        autoDelete: false,
                        arguments: null,
                        cancellationToken: stoppingToken);

                    // 2. Declare the main queue with x-dead-letter-exchange pointing to the DLQ
                    //    When BasicNack(requeue: false) is called, the message goes here
                    var queueArgs = new Dictionary<string, object?>
                    {
                        { "x-dead-letter-exchange", "" },       // default exchange
                        { "x-dead-letter-routing-key", dlqName } // route to dlq by name
                    };

                    await _channel.QueueDeclareAsync(
                        queue: queueName,
                        durable: true,          // survive restarts
                        exclusive: false,
                        autoDelete: false,
                        arguments: queueArgs,
                        cancellationToken: stoppingToken);

                    // Only process one message at a time — don't overwhelm the service
                    await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1,
                        global: false, cancellationToken: stoppingToken);

                    _logger.LogInformation(
                        "WorkflowService connected to RabbitMQ. Listening on {QueueName}. DLQ: {DlqName}",
                        queueName, dlqName);
                    break;
                }
                catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException ex)
                {
                    _logger.LogWarning(ex, "RabbitMQ not reachable. Retrying in 5 seconds...");
                    await Task.Delay(5000, stoppingToken);
                }
            }

            stoppingToken.ThrowIfCancellationRequested();

            // ── Set up consumer ──────────────────────────────────────────────
            var queueNameFinal = _configuration.GetValue<string>(QueueConfigKey)!;
            var consumer = new AsyncEventingBasicConsumer(_channel!);

            consumer.ReceivedAsync += async (_, ea) =>
            {
                // Extract CorrelationId from message properties
                // This was set by RabbitMqreferralMessageSender in WebAPI
                var correlationId = ea.BasicProperties.CorrelationId
                                    ?? Guid.NewGuid().ToString();

                // Push CorrelationId into Serilog context for the duration of this message
                using (LogContext.PushProperty("CorrelationId", correlationId))
                {
                    var success = await ProcessWithRetryAsync(ea, stoppingToken);

                    if (success)
                    {
                        // ACK — tell RabbitMQ to remove the message from the queue
                        await _channel!.BasicAckAsync(ea.DeliveryTag, multiple: false);
                        _logger.LogInformation(
                            "Referral message processed and ACKed. CorrelationId: {CorrelationId}",
                            correlationId);
                    }
                    else
                    {
                        // NACK without requeue — message goes to DLQ
                        await _channel!.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                        _logger.LogError(
                            "Referral message failed after {MaxRetries} retries. Sent to DLQ. CorrelationId: {CorrelationId}",
                            MaxRetries, correlationId);
                    }
                }
            };

            await _channel!.BasicConsumeAsync(
                queue: queueNameFinal,
                autoAck: false,   // manual ACK — message stays in queue until we ACK/NACK
                consumer: consumer,
                cancellationToken: stoppingToken);

            // Keep the BackgroundService alive
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        /// <summary>
        /// Tries to process the message up to MaxRetries times.
        /// Returns true on success, false after all retries are exhausted.
        /// </summary>
        private async Task<bool> ProcessWithRetryAsync(BasicDeliverEventArgs ea, CancellationToken ct)
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());

            for (var attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<setReferralDTO>(content);
                    if (dto is null) throw new InvalidOperationException("Deserialized DTO is null");

                    dto.Timestamp = DateTime.UtcNow;

                    // Create a new DI scope per message so scoped services (repositories) work correctly
                    using var scope = _scopeFactory.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    await mediator.Send(new createReferralCommand { setReferral = dto }, ct);
                    return true;
                }
                catch (Exception ex) when (attempt < MaxRetries)
                {
                    var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt)); // 2s, 4s
                    _logger.LogWarning(ex,
                        "Referral processing attempt {Attempt}/{MaxRetries} failed. Retrying in {Delay}s",
                        attempt, MaxRetries, delay.TotalSeconds);
                    await Task.Delay(delay, ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Referral processing failed on final attempt {Attempt}/{MaxRetries}",
                        attempt, MaxRetries);
                    return false;
                }
            }

            return false;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("WorkflowService RabbitMQ consumer stopping...");
            if (_channel is not null) await _channel.CloseAsync(cancellationToken: cancellationToken);
            if (_connection is not null) await _connection.CloseAsync(cancellationToken: cancellationToken);
            await base.StopAsync(cancellationToken);
        }
    }
}