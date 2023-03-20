using Events;
using GenerateIdentityServices.Commands;
using MassTransit;
using MediatR;

namespace GenerateIdentityServices.Consumers
{
    public class OrderRequestChangeStatusConsumer : IConsumer<ChengeIdentityNumberStatusEvent>
    {
        private readonly IMediator _mediator;
        public OrderRequestChangeStatusConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<ChengeIdentityNumberStatusEvent> context)
        {
            try
            {
                await _mediator.Send(new ChangeOrderRequestStatueCommand
                {
                    IdentityNumbers = context.Message.IdentityNumbers,
                    OrderRequestState = context.Message.OrderRequestState,
                });
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
