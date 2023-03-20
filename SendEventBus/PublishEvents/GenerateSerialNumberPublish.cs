using Events;
using Infra.Services.rabbitMq;
using SharedTatweerSendData.DTOs;

namespace SendEventBus.PublishEvents
{
    public class GenerateSerialNumberPublish
    {
        private readonly ISendNotifyServices<GenerateSerialNumberEvent> _publish;

        public GenerateSerialNumberPublish(
          ISendNotifyServices<GenerateSerialNumberEvent> publish)
        {
            _publish = publish;
        }

        public async Task PublishGenerateSerialNumber(string RequestIdentity)
        {
            await _publish.Notify(new GenerateSerialNumberEvent
            {
                RequestIdentity = RequestIdentity,
                PrintOutCenter = true,
            }, $"{QueueNames.GenerateSerialNumberQueue}");
        }
    }
}
