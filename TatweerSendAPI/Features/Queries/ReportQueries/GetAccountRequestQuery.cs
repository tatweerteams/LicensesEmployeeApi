using Infra;
using MediatR;
using SharedTatweerSendData.DTOs.ReportDTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.ReportQueries
{
    public class GetAccountRequestQuery : IRequest<ResultOperationDTO<PaginationDto<AccountRequestReportDTO>>>
    {
        public string BranchId { get; set; }
        public string AccountNo { get; set; }
        public string PhoneNo { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public BaseAccountType? AccountType { get; set; }

    }
    public class GetAccountRequestHandler : IRequestHandler<GetAccountRequestQuery, ResultOperationDTO<PaginationDto<AccountRequestReportDTO>>>
    {
        private readonly IReportServices _reportServices;

        public GetAccountRequestHandler(IReportServices reportServices)
        {
            _reportServices = reportServices;
        }
        public async Task<ResultOperationDTO<PaginationDto<AccountRequestReportDTO>>> Handle(GetAccountRequestQuery request, CancellationToken cancellationToken)
        {
            var result = await _reportServices.GetAccountRequest(request.BranchId, request.AccountNo, request.AccountType, request.PhoneNo, request.FromDate,
                request.ToDate, request.PageNo, request.PageSize);

            return ResultOperationDTO<PaginationDto<AccountRequestReportDTO>>.CreateSuccsessOperation(result);

        }
    }
}
