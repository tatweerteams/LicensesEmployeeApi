using Infra;
using MediatR;
using SharedTatweerSendData.Models.OrderRequestModels;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.OrderRequestCommands
{
    public class InsertRequestOrderCommand : IRequest<ResultOperationDTO<bool>>
    {
        public InsertOrderRequestModel Model { get; set; }
    }

    public class InsertRequestOrderHandler : IRequestHandler<InsertRequestOrderCommand, ResultOperationDTO<bool>>
    {

        private readonly IOrderRequestServices _orderRequestServices;
        public InsertRequestOrderHandler(IOrderRequestServices orderRequestServices)
        {
            _orderRequestServices = orderRequestServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(InsertRequestOrderCommand request, CancellationToken cancellationToken)
        {


            await _orderRequestServices.InsertOrderRequest(request.Model);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true,
                new string[] { "تم إنشاء طلب جديد , يجب إدراج قائمة من الحسابات" });
        }
    }
}
