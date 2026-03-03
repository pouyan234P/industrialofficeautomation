namespace webapi.RabbmitmqSender
{
    public interface IRabbitMQsearchMessageSender
    {
        void SendMessage(Object message, string queueName);
    }
}
