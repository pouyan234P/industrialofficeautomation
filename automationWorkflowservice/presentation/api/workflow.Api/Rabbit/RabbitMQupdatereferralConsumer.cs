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
    /// Consumes the "updatereferral" queue.
    /// Identical resilience pattern to RabbitMQreferralConsumer:
    /// DLQ, per-message retries, durable queue, structured logging, CorrelationId.
    /// </summary>
    public class RabbitMQupdatereferralConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RabbitMQupdatereferralConsumer> _logger;
        private IConnection? _connection;
        private IChannel? _channel;

        private const int MaxRetries = 3;
        private const string QueueConfigKey = "TopicAndQueueNames:updatereferral";

        public RabbitMQupdatereferralConsumer(
            IConfiguration configuration,
            IServiceScopeFactory scopeFactory,
            ILogger<RabbitMQupdatereferralConsumer> logger)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
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

                    var queueName = _configuration.GetValue<string>(QueueConfigKey)!;
                    var dlqName = $"{queueName}.dlq";

                    // Declare DLQ first
                    await _channel.QueueDeclareAsync(
                        queue: dlqName, durable: true, exclusive: false,
                        autoDelete: false, arguments: null, cancellationToken: stoppingToken);

                    // Main queue with dead-letter routing
                    var queueArgs = new Dictionary<string, object?>
                    {
                        { "x-dead-letter-exchange", "" },
                        { "x-dead-letter-routing-key", dlqName }
                    };

                    await _channel.QueueDeclareAsync(
                        queue: queueName, durable: true, exclusive: false,
                        autoDelete: false, arguments: queueArgs, cancellationToken: stoppingToken);

                    await _channel.BasicQosAsync(0, 1, false, stoppingToken);

                    _logger.LogInformation(
                        "UpdateReferral consumer connected. Queue: {QueueName}, DLQ: {DlqName}",
                        queueName, dlqName);
                    break;
                }
                catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException ex)
                {
                    _logger.LogWarning(ex, "RabbitMQ not reachable. Retrying in 5s...");
                    await Task.Delay(5000, stoppingToken);
                }
            }

            stoppingToken.ThrowIfCancellationRequested();

            var queueNameFinal = _configuration.GetValue<string>(QueueConfigKey)!;
            var consumer = new AsyncEventingBasicConsumer(_channel!);

            consumer.ReceivedAsync += async (_, ea) =>
            {
                var correlationId = ea.BasicProperties.CorrelationId ?? Guid.NewGuid().ToString();

                using (LogContext.PushProperty("CorrelationId", correlationId))
                {
                    var success = await ProcessWithRetryAsync(ea, stoppingToken);

                    if (success)
                    {
                        await _channel!.BasicAckAsync(ea.DeliveryTag, multiple: false);
                        _logger.LogInformation(
                            "UpdateReferral message ACKed. CorrelationId: {CorrelationId}", correlationId);
                    }
                    else
                    {
                        await _channel!.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                        _logger.LogError(
                            "UpdateReferral failed after {MaxRetries} retries. Sent to DLQ. CorrelationId: {CorrelationId}",
                            MaxRetries, correlationId);
                    }
                }
            };

            await _channel!.BasicConsumeAsync(
                queue: queueNameFinal,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task<bool> ProcessWithRetryAsync(BasicDeliverEventArgs ea, CancellationToken ct)
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());

            for (var attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<referralDTO>(content);
                    if (dto is null) throw new InvalidOperationException("Deserialized DTO is null");

                    using var scope = _scopeFactory.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Send(new updateReferralCommand { referral = dto }, ct);
                    return true;
                }
                catch (Exception ex) when (attempt < MaxRetries)
                {
                    var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt));
                    _logger.LogWarning(ex,
                        "UpdateReferral attempt {Attempt}/{MaxRetries} failed. Retrying in {Delay}s",
                        attempt, MaxRetries, delay.TotalSeconds);
                    await Task.Delay(delay, ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "UpdateReferral failed on final attempt {Attempt}/{MaxRetries}", attempt, MaxRetries);
                    return false;
                }
            }

            return false;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("UpdateReferral consumer stopping...");
            if (_channel is not null) await _channel.CloseAsync(cancellationToken: cancellationToken);
            if (_connection is not null) await _connection.CloseAsync(cancellationToken: cancellationToken);
            await base.StopAsync(cancellationToken);
        }
    }
}