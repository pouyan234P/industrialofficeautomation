namespace webapi.RabbmitmqSender
{
    public interface IRabbitMQreferralMessageSender
    {
        Task SendMessage(Object message, string queueName, string? correlationId = null);
        Task UpdateMessage(Object message, string queueName, string? correlationId = null);
    }
}
