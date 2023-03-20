using LogginServices.commands;
using MassTransit;
using MediatR;
using SharedTatweerSendData.Events;

namespace LogginServices.Consumers
{
    public class LogginDataEventConsumer : IConsumer<LogginDataEvent>
    {

        private readonly IMediator _mediator;
        public LogginDataEventConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<LogginDataEvent> context)
        {
            await _mediator.Send(new InsertEventDataCommand
            {
                DataEvent = context.Message
            });
        }
    }
}
