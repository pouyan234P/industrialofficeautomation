using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using workflow.Appliction.DTO;
using workflow.Appliction.Feature.referralFeature.request.Command;

namespace workflow.Api.Rabbit
{
    public class RabbitMQreferralConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private IConnection _connection;
        private IChannel _channel;
        private readonly ConnectionFactory _factory;

        public RabbitMQreferralConsumer(IConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;

            // Just set up the factory here, DO NOT connect yet.
            _factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                Password = "guest",
                UserName = "guest",
                // Optional: You can let the RabbitMQ client handle automatic recovery
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 1. Connect with a Retry Loop
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _connection = await _factory.CreateConnectionAsync(stoppingToken);
                    _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                    await _channel.QueueDeclareAsync(
                        queue: _configuration.GetValue<string>("TopicAndQueueNames:myreferral"),
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null,
                        cancellationToken: stoppingToken);

                    Console.WriteLine("Successfully connected to RabbitMQ!");
                    break; // Exit the retry loop once connected
                }
                catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException)
                {
                    Console.WriteLine("RabbitMQ not ready yet. Retrying in 5 seconds...");
                    await Task.Delay(5000, stoppingToken);
                }
            }

            stoppingToken.ThrowIfCancellationRequested();

            // 2. Set up the Consumer
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<setReferralDTO>(content);

                await Handlereferral(dto);

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await _channel.BasicConsumeAsync(
                queue: _configuration.GetValue<string>("TopicAndQueueNames:myreferral"),
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);
        }

        private async Task Handlereferral(setReferralDTO dto)
        {
            dto.Timestamp = DateTime.Now;
            var command = new createReferralCommand
            {
                setReferral = dto
            };
            await _mediator.Send(command);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // Clean up connections when the service stops
            if (_channel is not null) await _channel.CloseAsync(cancellationToken: cancellationToken);
            if (_connection is not null) await _connection.CloseAsync(cancellationToken: cancellationToken);
            await base.StopAsync(cancellationToken);
        }
    }
}