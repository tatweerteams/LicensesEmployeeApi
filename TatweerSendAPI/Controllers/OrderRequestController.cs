using Infra;
using Infra.Utili;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedTatweerSendData.DTOs;
using SharedTatweerSendData.Models.OrderRequestModels;
using TatweerSendAPI.Features.Commands.OrderRequestCommands;
using TatweerSendAPI.Features.Queries.OrderRequestQueries;
using TatweerSendAPI.Filters.OrderRequestFilter;

namespace TatweerSendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class OrderRequestController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly HelperUtili _helper;
        public OrderRequestController(IMediator mediator, HelperUtili helper)
        {
            _mediator = mediator;
            _helper = helper;

        }

        [Authorize(Roles = RolesUtili.OrderRequestNew + "," + RolesUtili.Administrator + "," + RolesUtili.AddOrderRequestOtherBranch)]
        [HttpPost("InsertOrderRequest")]
        [TypeFilter(typeof(InsertOrderRequestFilter))]
        public async Task<ResultOperationDTO<bool>> InsertOrderRequest([FromBody] InsertOrderRequestModel model, CancellationToken cancellationToken = default)
            => await _mediator.Send(new InsertRequestOrderCommand { Model = model });

        [Authorize(Roles = RolesUtili.OrderRequestNew + "," + RolesUtili.Administrator + "," + RolesUtili.AddOrderRequestOtherBranch)]
        [HttpGet("GetOrderRequests")]
        public async Task<ResultOperationDTO<PaginationDto<OrderRequestDTO>>> GetOrderRequests(string branchId, string note,
                            OrderRequestState? requestState, BaseAccountType? orderRequestType,
                            DateTime? fromDate, DateTime? toDate, int pageNo = 1, int pageSize = 30, string identityNo = null,
                            CancellationToken cancellationToken = default)
            => await _mediator.Send(new GetOrderRequestQuery
            {

                BranchId = branchId,
                Note = note,
                OrderRequestType = orderRequestType,
                RequestState = requestState,
                ToDate = toDate,
                FromDate = fromDate,
                PageNo = pageNo,
                PageSize = pageSize,
                IdentityNo = identityNo
            });

        [Authorize(Roles = RolesUtili.OrderRequestNew + "," + RolesUtili.Administrator + "," + RolesUtili.AddOrderRequestOtherBranch)]
        [HttpDelete("DeleteOrderRequest")]
        [TypeFilter(typeof(DeleteOrderRequestFilter))]
        public async Task<ResultOperationDTO<bool>> DeleteOrderRequest(string orderRequestId, CancellationToken cancellationToken = default)
            => await _mediator.Send(new DeleteOrderRequestCommand { OrderRequestId = orderRequestId });

        [Authorize(Roles = RolesUtili.OrderRequestNew + "," + RolesUtili.Administrator + "," + RolesUtili.AddOrderRequestOtherBranch)]
        [HttpPut("UpdateOrderRequest")]
        [TypeFilter(typeof(UpdateOrderRequestFilter))]
        public async Task<ResultOperationDTO<bool>> UpdateOrderRequest([FromBody] UpdateOrderRequestModel model, CancellationToken cancellationToken = default)
        => await _mediator.Send(new UpdateOrderRequestCommand { Model = model });

        [Authorize(Roles = RolesUtili.OrderRequestNew + "," + RolesUtili.Administrator + "," + RolesUtili.AddOrderRequestOtherBranch)]
        [HttpPut("SendOrderRequest")]
        [TypeFilter(typeof(SendRequestOrderFilter))]
        public async Task<ResultOperationDTO<bool>> SendOrderRequest(string orderRequestId, CancellationToken cancellationToken = default)
        => await _mediator.Send(new SendRequestOrderCommand
        {
            OrderRequestId = orderRequestId,
            UserType = _helper.GetCurrentUser().UserType.Value
        });


        [Authorize(Roles = RolesUtili.ApprovidRequest + "," + RolesUtili.Administrator + "," + RolesUtili.RejectRequest)]
        [HttpGet("GetOrderRequestPinding")]
        public async Task<ResultOperationDTO<PaginationDto<OrderRequestDTO>>> GetOrderRequestPinding(string branchId, string note,
                            BaseAccountType? orderRequestType,
                            DateTime? fromDate, DateTime? toDate, int pageNo = 1, int pageSize = 30,
                              string identityNo = null,
                            CancellationToken cancellationToken = default)
            => await _mediator.Send(new GetOrderRequestQuery
            {
                BranchId = branchId,
                UserType = _helper.GetCurrentUser().UserType,
                Note = note,
                OrderRequestType = orderRequestType,
                RequestState = OrderRequestState.Pinding,
                ToDate = toDate,
                FromDate = fromDate,
                PageNo = pageNo,
                PageSize = pageSize,
                OtherProccess = true,
                IdentityNo = identityNo,
            });

        [Authorize(Roles = RolesUtili.ApprovidRequest + "," + RolesUtili.Administrator)]
        [HttpPut("ApprovedOrderRequest")]
        [TypeFilter(typeof(ApprovedOrderRequestFilter))]
        public async Task<ResultOperationDTO<bool>> ApprovedOrderRequest(string orderRequestId, CancellationToken cancellationToken = default)
            => await _mediator.Send(new ApprovedOrderRequestCommand
            {
                OrderRequestId = orderRequestId,
                UserType = _helper.GetCurrentUser().UserType.Value
            });

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.RejectRequest)]
        [HttpPut("RejectOrderRequest")]
        [TypeFilter(typeof(RejectOrderRequestFilter))]
        public async Task<ResultOperationDTO<bool>> RejectOrderRequest(string orderRequestId, string RejectNote, CancellationToken cancellationToken = default)
            => await _mediator.Send(new RejectOrderRequestCommand
            {
                OrderRequestId = orderRequestId,
                UserType = _helper.GetCurrentUser().UserType.Value,
                RejectNote = RejectNote
            });

        [Authorize(Roles = RolesUtili.OrderRequestNew + "," + RolesUtili.Administrator + "," + RolesUtili.RejectRequest + "," + RolesUtili.AddOrderRequestOtherBranch)]
        [HttpGet("GetOrderRequestReject")]
        public async Task<ResultOperationDTO<PaginationDto<OrderRequestDTO>>> GetOrderRequestReject(string branchId, string note,
                            BaseAccountType? orderRequestType,
                            DateTime? fromDate, DateTime? toDate, int pageNo = 1, int pageSize = 30,
                            OrderRequestState? requestState = null,
                            string identityNo = null,
                            CancellationToken cancellationToken = default)
            => await _mediator.Send(new GetOrderRequestQuery
            {
                BranchId = branchId,
                UserType = _helper.GetCurrentUser().UserType,
                Note = note,
                OrderRequestType = orderRequestType,
                RequestState = requestState,
                ToDate = toDate,
                FromDate = fromDate,
                PageNo = pageNo,
                PageSize = pageSize,
                OtherProccess = true,
                IsReject = true,
                IdentityNo = identityNo,
            });

        [Authorize]
        [HttpGet("GetRejectNoteByOrderId")]
        public async Task<ResultOperationDTO<RejectNoteDTO>> GetRejectNoteByOrderId(string orderRequestId)
            => await _mediator.Send(new GetRejectNoteByOrderIdQuery
            {
                OrderRequestId = orderRequestId
            });
    }
}
