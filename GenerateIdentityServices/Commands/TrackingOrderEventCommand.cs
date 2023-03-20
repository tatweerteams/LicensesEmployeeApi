using GenerateIdentityServices.Services;
using Infra;
using MediatR;
using SharedTatweerSendData.Events;

namespace GenerateIdentityServices.Commands
{
    public class TrackingOrderEventCommand : IRequest<ResultOperationDTO<bool>>
    {
        public TrackingOrderEvent orderEvent { get; set; }
    }

    public class TrackingOrderEventHandler : IRequestHandler<TrackingOrderEventCommand, ResultOperationDTO<bool>>
    {
        private readonly ITrackingOrderRequestEventServices _requestEventServices;
        public TrackingOrderEventHandler(ITrackingOrderRequestEventServices requestEventServices)
        {
            _requestEventServices = requestEventServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(TrackingOrderEventCommand request, CancellationToken cancellationToken)
        {
            await _requestEventServices.InsertOrderRequestEvent(request.orderEvent);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true);
        }
    }
}
