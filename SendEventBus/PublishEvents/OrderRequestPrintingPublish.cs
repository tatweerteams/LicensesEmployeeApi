using Events;
using Infra;
using Infra.Services.rabbitMq;
using SharedTatweerSendData.DTOs;

namespace SendEventBus.PublishEvents
{
    public class OrderRequestChengeStauesPublish
    {
        private readonly ISendNotifyServices<ChengeIdentityNumberStatusEvent> _publish;

        public OrderRequestChengeStauesPublish(
          ISendNotifyServices<ChengeIdentityNumberStatusEvent> publish)
        {
            _publish = publish;
        }

        public async Task ChangeOrderRequestStatus(List<string> IdentityNumbers, OrderRequestState orderRequestState)
        {
            await _publish.Notify(new ChengeIdentityNumberStatusEvent
            {
                IdentityNumbers = IdentityNumbers,
                OrderRequestState = orderRequestState,
            }, $"{QueueNames.OrderRequestChangeStatusQueue}");
        }


    }
}
