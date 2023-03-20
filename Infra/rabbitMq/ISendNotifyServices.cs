namespace Infra.Services.rabbitMq
{
    public interface ISendNotifyServices<T>
    {
        Task Notify(T data, string queueName);
    }
}
