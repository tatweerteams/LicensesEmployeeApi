using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.OrderRequestCommands
{
    public class ApprovedOrderRequestCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string OrderRequestId { get; set; }
        public UserTypeState UserType { get; set; }
    }

    public class ApprovedOrderRequestHandler : IRequestHandler<ApprovedOrderRequestCommand, ResultOperationDTO<bool>>
    {
        private readonly IOrderRequestServices _orderRequestServices;
        public ApprovedOrderRequestHandler(IOrderRequestServices orderRequestServices)
        {
            _orderRequestServices = orderRequestServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(ApprovedOrderRequestCommand request, CancellationToken cancellationToken)
        {
            await _orderRequestServices.ApprovidRequest(request.OrderRequestId, request.UserType);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "لقد تم قبول هذا الطلب" });
        }
    }
}
