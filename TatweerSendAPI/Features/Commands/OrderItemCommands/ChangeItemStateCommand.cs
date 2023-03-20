using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.OrderItemCommands
{
    public class ChangeItemStateCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string OrderItemId { get; set; }
        public string OrderRequestId { get; set; }
        public UserTypeState UserType { get; set; }

    }

    public class ChangeItemStateHandler : IRequestHandler<ChangeItemStateCommand, ResultOperationDTO<bool>>
    {

        private readonly IOrderRequestItemServices _orderRequestItem;
        public ChangeItemStateHandler(IOrderRequestItemServices orderRequestItem)
        {
            _orderRequestItem = orderRequestItem;
        }
        public async Task<ResultOperationDTO<bool>> Handle(ChangeItemStateCommand request, CancellationToken cancellationToken)
        {
            await _orderRequestItem.ChangeItemState(request.OrderItemId, request.UserType);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "تمت العملية بنجاح" });

        }
    }
}
