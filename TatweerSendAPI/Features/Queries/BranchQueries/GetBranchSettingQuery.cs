using Infra;
using MediatR;
using SharedTatweerSendData.DTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.BranchQueries
{
    public class GetBranchSettingQuery : IRequest<ResultOperationDTO<BranchSettingDTO>>
    {
        public string BranchSetiingId { get; set; }
    }

    public class GetBranchSettingHandler : IRequestHandler<GetBranchSettingQuery, ResultOperationDTO<BranchSettingDTO>>
    {

        private readonly IBranchServices _branchServices;
        public GetBranchSettingHandler(IBranchServices branchServices)
        {
            _branchServices = branchServices;
        }
        public async Task<ResultOperationDTO<BranchSettingDTO>> Handle(GetBranchSettingQuery request, CancellationToken cancellationToken)
        {
            var result = await _branchServices.GetBranchSetting(request.BranchSetiingId);

            return ResultOperationDTO<BranchSettingDTO>.CreateSuccsessOperation(result);
        }
    }
}
