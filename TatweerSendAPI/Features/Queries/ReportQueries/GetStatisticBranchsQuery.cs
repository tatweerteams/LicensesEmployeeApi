using Infra;
using MediatR;
using SharedTatweerSendData.DTOs.ReportDTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.ReportQueries
{
    public class GetStatisticBranchsQuery : IRequest<ResultOperationDTO<PaginationDto<StatisticBranchDTO>>>
    {
        public string NameOrNumber { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string BankId { get; internal set; }
    }

    public class GetStatisticBranchHandler : IRequestHandler<GetStatisticBranchsQuery, ResultOperationDTO<PaginationDto<StatisticBranchDTO>>>
    {
        private readonly IReportServices _reportServices;

        public GetStatisticBranchHandler(IReportServices reportServices)
        {
            _reportServices = reportServices;
        }
        public async Task<ResultOperationDTO<PaginationDto<StatisticBranchDTO>>> Handle(GetStatisticBranchsQuery request, CancellationToken cancellationToken)
        {
            var result = await _reportServices.GetStatisticBranchs(request.NameOrNumber, request.BankId, request.PageNo, request.PageSize);

            return ResultOperationDTO<PaginationDto<StatisticBranchDTO>>.CreateSuccsessOperation(result);
        }
    }
}
