using Infra;
using MediatR;
using SharedTatweerSendData.DTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.OrderItemQueries
{
    public class GetAccountWithOutOrderItemQuery : IRequest<ResultOperationDTO<PaginationDto<AccountWithOutOrderItemDTO>>>
    {
        public string OrderRequestId { get; set; }
        public string AccounNoOrName { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAccountWithOutOrderItemHandler : IRequestHandler<GetAccountWithOutOrderItemQuery, ResultOperationDTO<PaginationDto<AccountWithOutOrderItemDTO>>>
    {

        private readonly IOrderRequestItemServices _requestItemServices;

        public GetAccountWithOutOrderItemHandler(IOrderRequestItemServices requestItemServices)
        {
            _requestItemServices = requestItemServices;
        }
        public async Task<ResultOperationDTO<PaginationDto<AccountWithOutOrderItemDTO>>> Handle(GetAccountWithOutOrderItemQuery request, CancellationToken cancellationToken)
        {
            var result = await _requestItemServices.
                GetAccountWithOutOrderItem(request.OrderRequestId, request.AccounNoOrName, request.PageNo, request.PageSize);

            return ResultOperationDTO<PaginationDto<AccountWithOutOrderItemDTO>>.CreateSuccsessOperation(result);
        }
    }
}
