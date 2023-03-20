using Events;
using GenerateIdentityServices.Commands;
using MassTransit;
using MediatR;

namespace GenerateIdentityServices.Consumers
{

    public class GenerateIdentityConsumer : IConsumer<GenereteIdentityNoEvent>
    {

        private readonly IMediator _mediator;
        public GenerateIdentityConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<GenereteIdentityNoEvent> context)
        {
            try
            {
                await _mediator.Send(new GenerateIdentityCommand
                {
                    OrderRequestId = context.Message.OrderRequestId,
                    UserType = context.Message.UserType,
                    EmployeeNo = context.Message.EmployeeNo,
                    UserId = context.Message.UserId,
                });
            }
            catch (Exception ex)
            {

                throw;
            }


        }


    }
}
