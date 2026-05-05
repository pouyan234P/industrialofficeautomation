using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;
using System.Text;

namespace webapi.RabbmitmqSender
{
    /// <summary>
    /// Publishes messages to RabbitMQ.
    ///
    /// Changes from original:
    ///   1. Queues are now declared durable: true — messages survive RabbitMQ restarts
    ///   2. CorrelationId is forwarded in BasicProperties so consumers can log it
    ///   3. ILogger replaces Console.WriteLine
    ///   4. Hostname read from IConfiguration instead of being hardcoded
    /// </summary>
    public class RabbitMqreferralMessageSender : IRabbitMQreferralMessageSender
    {
        private readonly string _hostname;
        private readonly string _username;
        private readonly string _password;

        // Inject IConfiguration so the hostname is not hardcoded
        public RabbitMqreferralMessageSender(IConfiguration configuration)
        {
            _hostname = configuration["RabbitMQ:Host"] ?? "192.168.1.173";
            _username = configuration["RabbitMQ:Username"] ?? "guest";
            _password = configuration["RabbitMQ:Password"] ?? "guest";
        }

        public async Task SendMessage(object message, string queueName, string? correlationId = null)
        {
            try
            {
                var factory = CreateFactory();
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();
                // 1. Create the arguments dictionary matching the existing queue
                var queueArgs = new Dictionary<string, object>
                {
                    { "x-dead-letter-exchange", "" },
                    { "x-dead-letter-routing-key", "myreferral.dlq" }
                };

                // 2. Pass the dictionary to the arguments parameter
                await channel.QueueDeclareAsync(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: queueArgs);
                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                // Forward CorrelationId so consumers can include it in their logs
                var props = new BasicProperties
                {
                    Persistent = true,   // message survives broker restart
                    CorrelationId = correlationId ?? Guid.NewGuid().ToString()
                };

                await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: queueName,
                    mandatory: false,
                    basicProperties: props,
                    body: body);

                Log.Information(
                    "Published message to {QueueName}. CorrelationId: {CorrelationId}",
                    queueName, props.CorrelationId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to publish message to {QueueName}", queueName);
                throw;
            }
        }

        public async Task UpdateMessage(object message, string queueName, string? correlationId = null)
        {
            try
            {
                var factory = CreateFactory();
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                var queueArgs = new Dictionary<string, object>
                {
                    { "x-dead-letter-exchange", "" },
                    { "x-dead-letter-routing-key", "updatereferral.dlq" }
                };

                // 2. Pass the dictionary to the arguments parameter
                await channel.QueueDeclareAsync(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: queueArgs);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                var props = new BasicProperties
                {
                    Persistent = true,
                    CorrelationId = correlationId ?? Guid.NewGuid().ToString()
                };

                await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: queueName,
                    mandatory: false,
                    basicProperties: props,
                    body: body);

                Log.Information(
                    "Updated message on {QueueName}. CorrelationId: {CorrelationId}",
                    queueName, props.CorrelationId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to publish update to {QueueName}", queueName);
                throw;
            }
        }

        private ConnectionFactory CreateFactory() => new()
        {
            HostName = _hostname,
            UserName = _username,
            Password = _password
        };
    }
}