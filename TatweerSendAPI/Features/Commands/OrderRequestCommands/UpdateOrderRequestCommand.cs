using Infra;
using MediatR;
using SharedTatweerSendData.Models.OrderRequestModels;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.OrderRequestCommands
{
    public class UpdateOrderRequestCommand : IRequest<ResultOperationDTO<bool>>
    {
        public UpdateOrderRequestModel Model { get; set; }
    }

    public class UpdateOrderRequestHandler : IRequestHandler<UpdateOrderRequestCommand, ResultOperationDTO<bool>>
    {
        private readonly IOrderRequestServices _orderRequestServices;
        public UpdateOrderRequestHandler(IOrderRequestServices orderRequestServices)
        {
            _orderRequestServices = orderRequestServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(UpdateOrderRequestCommand request, CancellationToken cancellationToken)
        {
            await _orderRequestServices.UpdateOrderRequest(request.Model);

            return ResultOperationDTO<bool>.
                CreateSuccsessOperation(true, message: new string[] { "تم عملية تعديل الطلب" });
        }
    }
}
