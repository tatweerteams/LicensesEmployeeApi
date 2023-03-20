using Events;
using Infra;
using Infra.Services.rabbitMq;
using SharedTatweerSendData.DTOs;

namespace SendEventBus.PublishEvents
{
    public class GenerateIdentityPublish
    {
        private readonly ISendNotifyServices<GenereteIdentityNoEvent> _publish;

        public GenerateIdentityPublish(
          ISendNotifyServices<GenereteIdentityNoEvent> publish)
        {
            _publish = publish;
        }

        public async Task PublishGenereteIdentity(string orderRequestId, string employeeNo, string UserId, UserTypeState userType, InputTypeState inputType)
        {
            await _publish.Notify(new GenereteIdentityNoEvent
            {
                OrderRequestId = orderRequestId,
                UserType = userType,
                InputType = inputType,
                UserId = UserId,
                EmployeeNo = employeeNo,
            }, $"{QueueNames.GenerateIdentityNameQueue}");
        }
    }
}
