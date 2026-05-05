using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;
using System.Text;

namespace webapi.RabbmitmqSender
{
    /// <summary>
    /// Publishes letter search documents to the "myels" queue.
    ///
    /// Changes from original:
    ///   1. Queue declared durable: true — survives RabbitMQ restarts
    ///   2. Messages marked Persistent = true — survive broker restart
    ///   3. CorrelationId forwarded in BasicProperties
    ///   4. RabbitMQ host read from IConfiguration instead of hardcoded
    ///   5. Structured Serilog logging instead of no logging
    ///   6. IConfiguration injected via constructor (was parameterless)
    /// </summary>
    public class RabbitMQsearchMessageSender : IRabbitMQsearchMessageSender
    {
        private readonly string _hostname;
        private readonly string _username;
        private readonly string _password;

        public RabbitMQsearchMessageSender(IConfiguration configuration)
        {
            _hostname = configuration["RabbitMQ:Host"] ?? "192.168.1.173";
            _username = configuration["RabbitMQ:Username"] ?? "guest";
            _password = configuration["RabbitMQ:Password"] ?? "guest";
        }

        public async Task SendMessage(object message, string queueName, string? correlationId = null)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password
                };

                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                // durable: true — queue survives RabbitMQ restarts
                var queueArgs = new Dictionary<string, object>
                {
                    { "x-dead-letter-exchange", "" },
                    { "x-dead-letter-routing-key", "myels.dlq" }
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
                    "Published search document to {QueueName}. CorrelationId: {CorrelationId}",
                    queueName, props.CorrelationId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to publish search document to {QueueName}", queueName);
                throw;
            }
        }
    }
}