using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.OrderRequestCommands
{
    public class SendRequestOrderCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string OrderRequestId { get; set; }
        public UserTypeState UserType { get; set; }
    }

    public class SendRequestOrderCommandHandler : IRequestHandler<SendRequestOrderCommand, ResultOperationDTO<bool>>
    {
        private readonly IOrderRequestServices _requestServices;
        public SendRequestOrderCommandHandler(IOrderRequestServices requestServices)
        {
            _requestServices = requestServices;
        }

        public async Task<ResultOperationDTO<bool>> Handle(SendRequestOrderCommand request, CancellationToken cancellationToken)
        {
            await _requestServices.SendOrderRequest(request.OrderRequestId, request.UserType);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { "تم إرسال الطلب" });
        }
    }
}
