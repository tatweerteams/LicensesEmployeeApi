using Infra;
using Infra.Utili;
using MediatR;
using SharedTatweerSendData.DTOs;
using TatweerSendServices.ExtensionServices;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.OrderRequestQueries
{
    public class GetOrderRequestQuery : IRequest<ResultOperationDTO<PaginationDto<OrderRequestDTO>>>
    {
        public OrderRequestState? RequestState { get; set; }
        public BaseAccountType? OrderRequestType { get; set; }
        public UserTypeState? UserType { get; set; }
        public string Note { get; set; }
        public string BranchId { get; set; }
        public string IdentityNo { get; set; }
        public string UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public bool OtherProccess { get; set; } = false;
        public bool IsReject { get; set; } = false;

    }

    public class GetOrderRequestQueryHandler :
        IRequestHandler<GetOrderRequestQuery, ResultOperationDTO<PaginationDto<OrderRequestDTO>>>
    {

        private readonly IOrderRequestServices _orderRequestServices;
        private readonly HelperUtili _helper;

        public GetOrderRequestQueryHandler(IOrderRequestServices orderRequestServices, HelperUtili helper)
        {
            _orderRequestServices = orderRequestServices;
            _helper = helper;
        }


        public async Task<ResultOperationDTO<PaginationDto<OrderRequestDTO>>> Handle(GetOrderRequestQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _helper.GetCurrentUser();
            var userBranch = request.BranchId ?? currentUser?.BranchId;
            request.UserId = currentUser?.UserID ?? "AdminSystem";
            request.UserType = currentUser.UserType;

            var branchId = request.UserType?.GetBranchIdOrderRequest(request.BranchId, userBranch);

            var result = await _orderRequestServices.GetAllOrderRequest(request.UserId, request.UserType, request.RequestState,
                request.OrderRequestType, request.Note, branchId, request.FromDate,
                request.ToDate, request.PageNo, request.PageSize, request.OtherProccess, request.IsReject, request.IdentityNo);

            return ResultOperationDTO<PaginationDto<OrderRequestDTO>>.CreateSuccsessOperation(result);
        }
    }


}
