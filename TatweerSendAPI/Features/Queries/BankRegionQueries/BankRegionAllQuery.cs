using Infra;
using MediatR;
using SharedTatweerSendData.DTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.BankRegionQueries
{
    public class BankRegionAllQuery : IRequest<ResultOperationDTO<PaginationDto<BankRegionDTO>>>
    {
        public string BankId { get; set; }
        public string RegionNo { get; set; }
        public string RegionName { get; set; }
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 30;
    }

    public class BankRegionAllHandler : IRequestHandler<BankRegionAllQuery, ResultOperationDTO<PaginationDto<BankRegionDTO>>>
    {
        private readonly IBankRegionServices _bankRegionServices;
        public BankRegionAllHandler(IBankRegionServices bankRegionServices)
        {
            _bankRegionServices = bankRegionServices;
        }
        public async Task<ResultOperationDTO<PaginationDto<BankRegionDTO>>> Handle(BankRegionAllQuery request, CancellationToken cancellationToken)
        {

            var result = await _bankRegionServices.
                GetAll(request.BankId, request.RegionName,
                       request.RegionNo, request.PageNo, request.PageSize, cancellationToken);

            return ResultOperationDTO<PaginationDto<BankRegionDTO>>.CreateSuccsessOperation(result);
        }
    }
}
