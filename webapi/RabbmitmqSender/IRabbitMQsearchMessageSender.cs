namespace webapi.RabbmitmqSender
{
    public interface IRabbitMQsearchMessageSender
    {
        Task SendMessage(Object message, string queueName, string? correlationId = null);
    }
}
