using Infra;
using MediatR;
using SharedTatweerSendData.DTOs.ReportDTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.ReportQueries
{
    public class GetOrderRequestPriteOutReportQuery : IRequest<ResultOperationDTO<PaginationDto<OrderRequestPriteOutDTO>>>
    {
        public string BranchId { get; set; }
        public string IdentityNo { get; set; }
        public string FromSerial { get; set; }
        public string ToSerial { get; set; }
        public BaseAccountType? OrderRequestType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }

    }
    public class GetOrderRequestPriteOutReportHandler : IRequestHandler<GetOrderRequestPriteOutReportQuery, ResultOperationDTO<PaginationDto<OrderRequestPriteOutDTO>>>
    {
        private readonly IReportServices _reportServices;

        public GetOrderRequestPriteOutReportHandler(IReportServices reportServices)
        {
            _reportServices = reportServices;
        }
        public async Task<ResultOperationDTO<PaginationDto<OrderRequestPriteOutDTO>>> Handle(GetOrderRequestPriteOutReportQuery request, CancellationToken cancellationToken)
        {
            var result = await _reportServices.GetOrderRequestPriteOutReport(request.BranchId, request.IdentityNo, request.OrderRequestType,
               request.FromSerial, request.ToSerial, request.FromDate, request.ToDate, request.PageNo, request.PageSize);

            return ResultOperationDTO<PaginationDto<OrderRequestPriteOutDTO>>.CreateSuccsessOperation(result);

        }
    }
}
