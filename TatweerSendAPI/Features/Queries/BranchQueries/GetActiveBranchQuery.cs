using Infra;
using MediatR;
using SharedTatweerSendData.DTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.BranchQueries
{
    public class GetActiveBranchQuery : IRequest<ResultOperationDTO<IReadOnlyList<ActiveBranchDTO>>>
    {
        public string numOrName { get; set; }
        public string BankRegionId { get; set; }
    }

    public class GetActiveBranchHandler : IRequestHandler<GetActiveBranchQuery, ResultOperationDTO<IReadOnlyList<ActiveBranchDTO>>>
    {
        private readonly IBranchServices _branchServices;
        public GetActiveBranchHandler(IBranchServices branchServices)
        {
            _branchServices = branchServices;
        }
        public async Task<ResultOperationDTO<IReadOnlyList<ActiveBranchDTO>>> Handle(GetActiveBranchQuery request, CancellationToken cancellationToken)
        {
            var result = await _branchServices.GetActiveBranchs(request.numOrName, request.BankRegionId);
            return ResultOperationDTO<IReadOnlyList<ActiveBranchDTO>>.CreateSuccsessOperation(result);
        }
    }
}
