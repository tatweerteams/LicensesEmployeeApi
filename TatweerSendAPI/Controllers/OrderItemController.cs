using Infra;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedTatweerSendData.DTOs;
using SharedTatweerSendData.Models.OrderItemModel;
using TatweerSendAPI.Features.Commands.OrderItemCommands;
using TatweerSendAPI.Features.Queries.OrderItemQueries;
using TatweerSendAPI.Filters.OrderItemFilter;

namespace TatweerSendAPI.Controllers
{
    [Route("api/[controller]")]
    public class OrderItemController : BaseController
    {
        private readonly IMediator _mediator;
        public OrderItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("GetOrderItems")]
        public async Task<ResultOperationDTO<OrderItemResultDTO>>
           GetOrderItems(string orderRequestId, string accounNoOrName,
            string serialFrom, int? quentity, OrderItemState? orderItemState, int pageNo = 1, int pageSize = 30)
           => await _mediator.Send(new GetOrderItemQuery
           {
               OrderRequestId = orderRequestId,
               SerialFrom = serialFrom,
               AccounNoOrName = accounNoOrName,
               QrderItemState = orderItemState,
               Quentity = quentity,
               PageNo = pageNo,
               PageSize = pageSize
           });

        [Authorize]
        [HttpGet("GetAccountWithOutOrderItem")]
        public async Task<ResultOperationDTO<PaginationDto<AccountWithOutOrderItemDTO>>>
           GetAccountWithOutOrderItem(string orderRequestId, string accounNoOrName, int pageNo = 1, int pageSize = 30)
           => await _mediator.Send(new GetAccountWithOutOrderItemQuery
           {
               OrderRequestId = orderRequestId,
               AccounNoOrName = accounNoOrName,
               PageNo = pageNo,
               PageSize = pageSize
           });

        [Authorize]

        [HttpPost("InsertOrderItemRequest")]
        [TypeFilter(typeof(InsertOrderItemFilter))]
        public async Task<ResultOperationDTO<bool>> InsertOrderItemRequest([FromBody] InsertOrderItemModel model, CancellationToken cancellationToken = default)
          => await _mediator.Send(new InsertOrderItemCommand { OrderItem = model });

        [Authorize]
        [HttpPut("UpdateOrderItem")]
        [TypeFilter(typeof(UpdateOrderItemFilter))]
        public async Task<ResultOperationDTO<bool>> UpdateOrderItem([FromBody] UpdateOrderItemModel model, CancellationToken cancellationToken = default)
          => await _mediator.Send(new UpdateOrderItemCommand { UpdateModel = model });

        [Authorize]
        [HttpDelete("DeleteOrderItem")]
        [TypeFilter(typeof(DeleteOrderItemFilter))]
        public async Task<ResultOperationDTO<bool>> DeleteOrderItem(string orderItemId, CancellationToken cancellationToken = default)
          => await _mediator.Send(new DeleteOrderItemCommand { OrderItemId = orderItemId });

        [Authorize]
        [HttpPut("ChangeItemState")]
        [TypeFilter(typeof(ChangeItemStateFilter))]
        public async Task<ResultOperationDTO<bool>> ChangeItemState(string orderItemId, CancellationToken cancellationToken = default)
          => await _mediator.Send(new ChangeItemStateCommand
          {
              OrderItemId = orderItemId,
              UserType = UserTypeState.Employee

          });

    }

}
