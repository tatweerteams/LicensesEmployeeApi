using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.OrderRequestCommands
{
    public class DeleteOrderRequestCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string OrderRequestId { get; set; }
    }

    public class DeleteOrderRequestHandler : IRequestHandler<DeleteOrderRequestCommand, ResultOperationDTO<bool>>
    {

        private readonly IOrderRequestServices _orderRequestServices;
        public DeleteOrderRequestHandler(IOrderRequestServices orderRequestServices)
        {
            _orderRequestServices = orderRequestServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(DeleteOrderRequestCommand request, CancellationToken cancellationToken)
        {
            await _orderRequestServices.DeleteOrderRequest(request.OrderRequestId);

            return ResultOperationDTO<bool>.
                CreateSuccsessOperation(true, message: new string[] { "تم إلغاء الطلبية" });
        }
    }
}
