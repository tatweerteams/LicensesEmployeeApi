using Infra;
using MediatR;
using SharedTatweerSendData.DTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.RegionQueries
{
    public class GetRegionListQuery : IRequest<ResultOperationDTO<IReadOnlyList<RegionDTO>>>
    {
        public string RegionName { get; set; }
        public string RegionNumber { get; set; }
        public CancellationToken cancellationToken { get; set; } = default;
    }

    public class GetRegionHandler : IRequestHandler<GetRegionListQuery, ResultOperationDTO<IReadOnlyList<RegionDTO>>>
    {
        private readonly IRegionServices _regionServices;
        public GetRegionHandler(IRegionServices regionServices)
        {
            _regionServices = regionServices;
        }

        public async Task<ResultOperationDTO<IReadOnlyList<RegionDTO>>> Handle(GetRegionListQuery request, CancellationToken cancellationToken)
        {
            var result = await _regionServices.GetRegions(request.RegionName, request.RegionNumber, cancellationToken);
            return ResultOperationDTO<IReadOnlyList<RegionDTO>>.CreateSuccsessOperation(result);
        }
    }
}
