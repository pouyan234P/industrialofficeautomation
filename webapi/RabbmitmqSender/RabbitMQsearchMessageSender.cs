using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace webapi.RabbmitmqSender
{
    public class RabbitMQsearchMessageSender : IRabbitMQsearchMessageSender
    {
        private readonly string _hostname;
        private readonly string _username;
        private readonly string _password;
        private IConnection _connection;
        public RabbitMQsearchMessageSender()
        {
            _hostname = "192.168.1.173";
            _password = "guest";
            _username = "guest";
        }
        public async void SendMessage(object message, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                Password = _password,
                UserName = _username
            };
            _connection =await factory.CreateConnectionAsync();
            using var channel = await _connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queueName, false, false, false, null);
            var json=JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);

        }
    }
}
