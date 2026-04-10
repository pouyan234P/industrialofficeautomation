namespace webapi.RabbmitmqSender
{
    public interface IRabbitMQreferralMessageSender
    {
        void SendMessage(Object message, string queueName);
        void UpdateMessage(Object message, string queueName);
    }
}
