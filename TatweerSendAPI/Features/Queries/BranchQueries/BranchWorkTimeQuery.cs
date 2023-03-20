using Infra;
using MediatR;
using SharedTatweerSendData.DTOs.BranchDTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.BranchQueries
{
    public class BranchWorkTimeQuery : IRequest<ResultOperationDTO<IReadOnlyList<BranchWorkTimeDTO>>>
    {
        public string BranchId { get; set; }
    }

    public class BranchWorkTimeHandler : IRequestHandler<BranchWorkTimeQuery, ResultOperationDTO<IReadOnlyList<BranchWorkTimeDTO>>>
    {

        private readonly IBranchServices _branchServices;
        public BranchWorkTimeHandler(IBranchServices branchServices)
        {
            _branchServices = branchServices;
        }
        public async Task<ResultOperationDTO<IReadOnlyList<BranchWorkTimeDTO>>> Handle(BranchWorkTimeQuery request, CancellationToken cancellationToken)
        {
            var result = await _branchServices.GetBranchWorkTimes(request.BranchId);

            return ResultOperationDTO<IReadOnlyList<BranchWorkTimeDTO>>.CreateSuccsessOperation(result);
        }
    }
}
