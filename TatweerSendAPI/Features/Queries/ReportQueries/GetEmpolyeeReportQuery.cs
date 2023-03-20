using Infra;
using MediatR;
using SharedTatweerSendData.DTOs.ReportDTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.ReportQueries
{
    public class GetEmpolyeeReportQuery : IRequest<ResultOperationDTO<PaginationDto<EmployeeReportDTO>>>
    {
        public string BranchId { get; set; }
        public string EmployeeNo { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }

    }
    public class GetEmpolyeeReportHandler : IRequestHandler<GetEmpolyeeReportQuery, ResultOperationDTO<PaginationDto<EmployeeReportDTO>>>
    {
        private readonly IReportServices _reportServices;

        public GetEmpolyeeReportHandler(IReportServices reportServices)
        {
            _reportServices = reportServices;
        }
        public async Task<ResultOperationDTO<PaginationDto<EmployeeReportDTO>>> Handle(GetEmpolyeeReportQuery request, CancellationToken cancellationToken)
        {
            var result = await _reportServices.GetEmpolyeeReport(request.BranchId, request.EmployeeNo,
                 request.FromDate, request.ToDate, request.PageNo, request.PageSize);

            return ResultOperationDTO<PaginationDto<EmployeeReportDTO>>.CreateSuccsessOperation(result);

        }
    }
}
