using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.OrderItemCommands
{
    public class DeleteOrderItemCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string OrderItemId { get; set; }
    }

    public class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand, ResultOperationDTO<bool>>
    {
        private readonly IOrderRequestItemServices _requestItemServices;
        public DeleteOrderItemCommandHandler(IOrderRequestItemServices requestItemServices)
        {
            _requestItemServices = requestItemServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
        {
            await _requestItemServices.DeleteOrderItem(request.OrderItemId);
            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { "تم إلغاء الحساب من الطلبية" });
        }
    }
}
