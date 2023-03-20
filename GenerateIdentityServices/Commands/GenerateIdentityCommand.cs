using Events;
using FilterAttributeWebAPI.Common;
using GenerateIdentityServices.Services;
using Infra;
using Infra.Services.rabbitMq;
using MediatR;
using SendEventBus.PublishEvents;
using SharedTatweerSendData.DTOs;

namespace GenerateIdentityServices.Commands
{
    public class GenerateIdentityCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string OrderRequestId { get; set; }
        public string UserId { get; set; }
        public string EmployeeNo { get; set; }
        public UserTypeState UserType { get; set; }
    }

    public class GenerateIdentityCommandHandler : IRequestHandler<GenerateIdentityCommand, ResultOperationDTO<bool>>
    {
        private readonly IGenerateIdentityRequestServices _generateIdentityServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TrackingOrderEventPublish _sendTrackingOrderEvent;
        private readonly GenerateSerialNumberPublish _generateSerialNumberPublish;
        private readonly ISendNotifyServices<ReciveOrderRequestEvent> _sendOrderEvent;
        public GenerateIdentityCommandHandler(
            IGenerateIdentityRequestServices generateIdentityServices,
            IUnitOfWork unitOfWork,
            TrackingOrderEventPublish sendTrackingOrderEvent,
            ISendNotifyServices<ReciveOrderRequestEvent> sendOrderEvent,
            GenerateSerialNumberPublish generateSerialNumberPublish)
        {
            _generateIdentityServices = generateIdentityServices;
            _unitOfWork = unitOfWork;
            _sendTrackingOrderEvent = sendTrackingOrderEvent;
            _sendOrderEvent = sendOrderEvent;
            _generateSerialNumberPublish = generateSerialNumberPublish;
        }

        public async Task<ResultOperationDTO<bool>> Handle(GenerateIdentityCommand request, CancellationToken cancellationToken)
        {

            var orderRequest = await _generateIdentityServices.GetOrderRequest(request.OrderRequestId);
            if (orderRequest == null)
                throw new ApplicationEx("بيانات الطلب غير موجودة");

            var branchResult = await _generateIdentityServices.GetBranch(orderRequest.BranchId);

            orderRequest.IdentityNumber = branchResult.LastCountChekBook.
                GenerateIdentityNumber((int)orderRequest.OrderRequestType, int.Parse(orderRequest.BranchId), branchResult.BranchNo);

            branchResult.LastCountChekBook++;
            orderRequest.ChCount = branchResult.LastCountChekBook;

            orderRequest.OrderRequestState = orderRequest.PrintOutCenter ?
                OrderRequestState.PrintOutCenter :
                OrderRequestState.SendRequestBranch;

            await _unitOfWork.SaveChangeAsync();

            if (orderRequest.PrintOutCenter)
            {
                await _generateSerialNumberPublish.PublishGenerateSerialNumber(RequestIdentity: orderRequest.IdentityNumber);
            }

            await _sendTrackingOrderEvent.PublishTrackingEvent(
                employeeNo: request.EmployeeNo,
                userId: request.UserId,
                publishDate: DateTime.Now,
                userType: request.UserType,
                orderRequestState: orderRequest.OrderRequestState,
                orderRequestId: request.OrderRequestId
            );

            if (orderRequest.OrderRequestState.Equals(OrderRequestState.SendRequestBranch))
            {
                var result = await _generateIdentityServices.GetOrderItemEvntes(orderRequest.Id);
                if (result.Any())
                    await _sendOrderEvent.Notify(new ReciveOrderRequestEvent
                    {
                        OrderItems = result
                    }, $"{QueueNames.ReciveOrderRequestQueue}");
            }

            // send event with data to DataBase Center ex object{OrderRequest orderRequest;} 
            return ResultOperationDTO<bool>.CreateSuccsessOperation(true);

        }
    }
}
