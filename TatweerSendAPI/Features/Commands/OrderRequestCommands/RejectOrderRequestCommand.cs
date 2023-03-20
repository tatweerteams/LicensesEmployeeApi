using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.OrderRequestCommands
{
    public class RejectOrderRequestCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string OrderRequestId { get; set; }
        public string RejectNote { get; set; }
        public UserTypeState UserType { get; set; }
    }

    public class RejectOrderRequestHandler : IRequestHandler<RejectOrderRequestCommand, ResultOperationDTO<bool>>
    {
        private readonly IOrderRequestServices _orderRequestServices;
        public RejectOrderRequestHandler(IOrderRequestServices orderRequestServices)
        {
            _orderRequestServices = orderRequestServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(RejectOrderRequestCommand request, CancellationToken cancellationToken)
        {
            await _orderRequestServices.RejectRequest(request.OrderRequestId, request.RejectNote, request.UserType);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "لقد تم رفض هذا الطلب" });
        }
    }
}
