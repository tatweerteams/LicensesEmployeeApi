using Infra;
using MediatR;
using SharedTatweerSendData.Models.OrderItemModel;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.OrderItemCommands
{
    public class UpdateOrderItemCommand : IRequest<ResultOperationDTO<bool>>
    {
        public UpdateOrderItemModel UpdateModel { get; set; }
    }
    public class UpdateOrderItemCommandHandler : IRequestHandler<UpdateOrderItemCommand, ResultOperationDTO<bool>>
    {
        private readonly IOrderRequestItemServices _requestItemServices;
        public UpdateOrderItemCommandHandler(IOrderRequestItemServices requestItemServices)
        {
            _requestItemServices = requestItemServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
        {
            await _requestItemServices.UpdateOrderItem(request.UpdateModel);

            // NOTE: send notify event Update Account in table Account 

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { "تم العملية تعديل بيانات الحساب" });
        }
    }
}
