namespace webapi.RabbmitmqSender
{
    public interface IRabbitMQreferralMessageSender
    {
        void SendMessage(Object message, string queueName);
    }
}
