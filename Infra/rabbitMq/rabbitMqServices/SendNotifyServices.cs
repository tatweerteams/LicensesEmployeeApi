using Infra.Services.rabbitMq;
using MassTransit;


namespace infra.rabbitMq.rabbitMqServices
{
    public class SendNotifyServices<T> : ISendNotifyServices<T>
    {
        private readonly IBusControl _bus;
        public SendNotifyServices(IBusControl bus)
        {
            _bus = bus;
        }
        public async Task Notify(T data, string queueName)
        {
            var uri = new Uri($"rabbitmq://localhost/{queueName}");
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(data);
        }


    }
}
