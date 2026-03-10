
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using searchengine.Application.Feature.leatterFeature.request.Command;
using searchengine.domain;
using System.Text;

namespace searchengine.Api.RabbitMQ
{
    public class RabbitMQsearchConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection _connection;
        private IChannel _channel;

        public RabbitMQsearchConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
            var factory = new ConnectionFactory
            {
                HostName = "192.168.1.173",
                Password = "guest",
                UserName = "guest"
            };

            // Await the asynchronous method to resolve the CS0266 error
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
            _channel.QueueDeclareAsync(_configuration.GetValue<string>("TopicAndQueueNames:myels"), false, false, false, null).GetAwaiter().GetResult();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<LetterSearchDocument>(content);
                Handlesearch(dto).GetAwaiter().GetResult();

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };
            _channel.BasicConsumeAsync(_configuration.GetValue<string>("TopicAndQueueNames:myels"), false, consumer);
            return Task.CompletedTask;
        }
        private async Task Handlesearch(LetterSearchDocument model)
        {
            try
            {
                var command = new createandupdateLetterCommand
                {
                    document = model
                };

                // 3. CHANGE: Create a fresh scope for THIS specific RabbitMQ message
                using (var scope = _scopeFactory.CreateScope())
                {
                    // Resolve a fresh IMediator from the new scope
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    // Now this will successfully resolve your Scoped Repository!
                    var result = await mediator.Send(command);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
