using Infra;
using MediatR;
using SharedTatweerSendData.Models.OrderItemModel;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.OrderItemCommands
{
    public class InsertOrderItemCommand : IRequest<ResultOperationDTO<bool>>
    {
        public InsertOrderItemModel OrderItem { get; set; }
    }

    public class InsertOrderItemCommandHandler : IRequestHandler<InsertOrderItemCommand, ResultOperationDTO<bool>>
    {

        private readonly IOrderRequestItemServices _requestItemServices;
        public InsertOrderItemCommandHandler(IOrderRequestItemServices requestItemServices)
        {
            _requestItemServices = requestItemServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(InsertOrderItemCommand request, CancellationToken cancellationToken)
        {
            await _requestItemServices.InsertOrderItem(request.OrderItem);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { "تم إضافة الحساب" });
        }
    }
}
