using Infra;
using MediatR;
using SharedTatweerSendData.DTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.OrderRequestQueries
{
    public class GetRejectNoteByOrderIdQuery : IRequest<ResultOperationDTO<RejectNoteDTO>>
    {
        public string OrderRequestId { get; set; }
    }

    public class GetRejectNoteByOrderIdHandler : IRequestHandler<GetRejectNoteByOrderIdQuery, ResultOperationDTO<RejectNoteDTO>>
    {
        private readonly IOrderRequestServices _orderRequestServices;
        public GetRejectNoteByOrderIdHandler(IOrderRequestServices orderRequestServices)
        {
            _orderRequestServices = orderRequestServices;
        }
        public async Task<ResultOperationDTO<RejectNoteDTO>> Handle(GetRejectNoteByOrderIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _orderRequestServices.
                    GetRejectNoteByOrderId(request.OrderRequestId);

            return ResultOperationDTO<RejectNoteDTO>.CreateSuccsessOperation(result);

        }
    }
}
