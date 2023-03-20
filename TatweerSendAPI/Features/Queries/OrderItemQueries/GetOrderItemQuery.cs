using Infra;
using MediatR;
using SharedTatweerSendData.DTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.OrderItemQueries
{
    public class GetOrderItemQuery : IRequest<ResultOperationDTO<OrderItemResultDTO>>
    {
        public string OrderRequestId { get; set; }
        public string AccounNoOrName { get; set; }
        public string SerialFrom { get; set; }
        public int? Quentity { get; set; }
        public OrderItemState? QrderItemState { get; set; }
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 30;
    }

    public class GetOrderItemQueryHandler : IRequestHandler<GetOrderItemQuery, ResultOperationDTO<OrderItemResultDTO>>
    {
        private readonly IOrderRequestItemServices _requestItemServices;
        public GetOrderItemQueryHandler(IOrderRequestItemServices requestItemServices)
        {
            _requestItemServices = requestItemServices;
        }
        public async Task<ResultOperationDTO<OrderItemResultDTO>> Handle(GetOrderItemQuery request, CancellationToken cancellationToken)
        {
            var result = await _requestItemServices.GetOrderItems(request.OrderRequestId, request.AccounNoOrName, request.SerialFrom,
                                                                request.Quentity, request.QrderItemState, request.PageNo, request.PageSize);

            return ResultOperationDTO<OrderItemResultDTO>.CreateSuccsessOperation(result);
        }
    }
}
