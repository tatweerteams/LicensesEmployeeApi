using GenerateIdentityServices.Commands;
using MassTransit;
using MediatR;
using SharedTatweerSendData.Events;

namespace GenerateIdentityServices.Consumers
{
    public class TrackingOrderRequestEventConsumer : IConsumer<TrackingOrderEvent>
    {
        private readonly IMediator _mediator;

        public TrackingOrderRequestEventConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<TrackingOrderEvent> context)
        {
            try
            {
                var requestEvent = context.Message;
                await _mediator.Send(new TrackingOrderEventCommand
                {
                    orderEvent = context.Message,
                });


            }
            catch (Exception ex)
            {

                throw;
            }

        }

    }
}
