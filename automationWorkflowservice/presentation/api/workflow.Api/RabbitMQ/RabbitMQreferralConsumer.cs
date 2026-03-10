
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using workflow.Appliction.DTO;
using workflow.Appliction.Feature.referralFeature.request.Command;

namespace workflow.Api.RabbitMQ
{
    public class RabbitMQreferralConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private IConnection _connection;
        private IChannel _channel;

        public RabbitMQreferralConsumer(IConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
            var factory = new ConnectionFactory
            {
                HostName = "192.168.1.173",
                Password = "guest",
                UserName = "guest"
            };

            // Await the asynchronous method to resolve the CS0266 error
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel=_connection.CreateChannelAsync().GetAwaiter().GetResult();
            _channel.QueueDeclareAsync(_configuration.GetValue<string>("TopicAndQueueNames:myreferral"), false, false, false, null).GetAwaiter().GetResult();
        }

      

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync +=async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<setReferralDTO>(content);
                Handlereferral(dto).GetAwaiter().GetResult();

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };
            _channel.BasicConsumeAsync(_configuration.GetValue<string>("TopicAndQueueNames:myreferral"), false, consumer);
            return Task.CompletedTask;
        }

        private async Task Handlereferral(setReferralDTO dto)
        {
            dto.Timestamp = DateTime.Now;
            var command = new createReferralCommand
            {
                setReferral = dto
            };
            var result = await _mediator.Send(command);
        }
    }
}
