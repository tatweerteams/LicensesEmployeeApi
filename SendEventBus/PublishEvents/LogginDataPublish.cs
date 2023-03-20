using Infra.Services.rabbitMq;
using SharedTatweerSendData.DTOs;
using SharedTatweerSendData.Events;

namespace SendEventBus.PublishEvents
{
    public class LogginDataPublish
    {
        private readonly ISendNotifyServices<LogginDataEvent> _publish;

        public LogginDataPublish(
          ISendNotifyServices<LogginDataEvent> publish)
        {
            _publish = publish;
        }

        public async Task PublishEventData(LogginDataEvent @event)
        {
            await _publish.Notify(@event, $"{QueueNames.LogginDataEventQueue}");
        }
    }
}
