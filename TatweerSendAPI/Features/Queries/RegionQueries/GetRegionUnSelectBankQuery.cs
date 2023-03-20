using Infra;
using MediatR;
using SharedTatweerSendData.DTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.RegionQueries
{
    public class GetRegionUnSelectBankQuery : IRequest<ResultOperationDTO<IReadOnlyList<RegionDTO>>>
    {
        public string BankId { get; set; }
        public CancellationToken cancellationToken { get; set; } = default;
    }

    public class GetRegionUnSelectBankHandler : IRequestHandler<GetRegionUnSelectBankQuery, ResultOperationDTO<IReadOnlyList<RegionDTO>>>
    {
        private readonly IRegionServices _regionServices;
        public GetRegionUnSelectBankHandler(IRegionServices regionServices)
        {
            _regionServices = regionServices;
        }

        public async Task<ResultOperationDTO<IReadOnlyList<RegionDTO>>> Handle(GetRegionUnSelectBankQuery request, CancellationToken cancellationToken)
        {
            var result = await _regionServices.GetRegionUnSelectedBank(request.BankId, cancellationToken);
            return ResultOperationDTO<IReadOnlyList<RegionDTO>>.CreateSuccsessOperation(result);
        }
    }
}
