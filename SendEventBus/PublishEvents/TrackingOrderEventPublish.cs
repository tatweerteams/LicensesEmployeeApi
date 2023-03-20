using Infra;
using Infra.Services.rabbitMq;
using SharedTatweerSendData.DTOs;
using SharedTatweerSendData.Events;

namespace SendEventBus.PublishEvents
{

    public class TrackingOrderEventPublish
    {
        private readonly ISendNotifyServices<TrackingOrderEvent> _sendTrackingOrderEvent;

        public TrackingOrderEventPublish(
          ISendNotifyServices<TrackingOrderEvent> sendTrackingOrderEvent)
        {
            _sendTrackingOrderEvent = sendTrackingOrderEvent;
        }

        public async Task PublishTrackingEvent(string employeeNo, string userId, DateTime publishDate,
            UserTypeState userType, OrderRequestState orderRequestState, string orderRequestId, string rejectNote = null)
        {
            await _sendTrackingOrderEvent.Notify(new TrackingOrderEvent
            {
                EmployeeNo = employeeNo,
                UserId = userId,
                OrderRequestId = orderRequestId,
                OrderCreationDate = publishDate,
                UserType = userType,
                OrderRequestState = orderRequestState,
                RejectNote = rejectNote,

            }, $"{QueueNames.TrackingOrderRequestEventQueue}");
        }
    }

    public class SendOrderRequestItemPublish<TEvent>
    {
        private readonly ISendNotifyServices<TEvent> _publish;
        public SendOrderRequestItemPublish(ISendNotifyServices<TEvent> publish)
        {
            _publish = publish;
        }

        public async Task PublishOrderRequestReciveEvent(TEvent @event)
        {
            await _publish.Notify(@event, $"{QueueNames.ReciveOrderRequestQueue}");
        }

    }
}
