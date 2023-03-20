using Infra;
using MediatR;
using SharedTatweerSendData.DTOs.ReportDTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.ReportQueries
{
    public class GetBranchReportQuery : IRequest<ResultOperationDTO<PaginationDto<BranchOrderReportDTO>>>
    {
        public string BranchId { get; set; }
        public string IdentityNo { get; set; }
        public BaseAccountType? OrderRequestType { get; set; }
        public OrderRequestState? OrderRequestState { get; set; }
        public InputTypeState? InputType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }

    }
    public class GetBranchReportHandler : IRequestHandler<GetBranchReportQuery, ResultOperationDTO<PaginationDto<BranchOrderReportDTO>>>
    {
        private readonly IReportServices _reportServices;

        public GetBranchReportHandler(IReportServices reportServices)
        {
            _reportServices = reportServices;
        }
        public async Task<ResultOperationDTO<PaginationDto<BranchOrderReportDTO>>> Handle(GetBranchReportQuery request, CancellationToken cancellationToken)
        {
            var result = await _reportServices.GetBranchReport(request.BranchId, request.IdentityNo, request.OrderRequestState, request.OrderRequestType,
               request.InputType, request.FromDate, request.ToDate, request.PageNo, request.PageSize);

            return ResultOperationDTO<PaginationDto<BranchOrderReportDTO>>.CreateSuccsessOperation(result);

        }
    }
}
