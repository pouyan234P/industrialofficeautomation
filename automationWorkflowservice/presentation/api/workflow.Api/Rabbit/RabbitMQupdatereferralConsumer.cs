
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using workflow.Appliction.DTO;
using workflow.Appliction.Feature.referralFeature.request.Command;

namespace workflow.Api.Rabbit
{
    public class RabbitMQupdatereferralConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private IConnection _connection;
        private IChannel _channel;
        private readonly ConnectionFactory _factory;
        public RabbitMQupdatereferralConsumer(IConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
            _factory = new ConnectionFactory
            {
                HostName = "192.168.1.173",
                Password = "guest",
                UserName = "guest",
                // Optional: You can let the RabbitMQ client handle automatic recovery
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _connection = await _factory.CreateConnectionAsync(stoppingToken);
                    _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                    await _channel.QueueDeclareAsync(
                        queue: _configuration.GetValue<string>("TopicAndQueueNames:updatereferral")!,
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
                var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<referralDTO>(content);

                await Handlereferral(dto!);

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await _channel.BasicConsumeAsync(
                queue: _configuration.GetValue<string>("TopicAndQueueNames:updatereferral")!,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);
        }

        private async Task Handlereferral(referralDTO dto)
        {
            var command = new updateReferralCommand
            {
                referral = dto
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
