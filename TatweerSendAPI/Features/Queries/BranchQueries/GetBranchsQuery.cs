using Infra;
using MediatR;
using SharedTatweerSendData.DTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.BranchQueries
{
    public class GetBranchsQuery : IRequest<ResultOperationDTO<PaginationDto<BranchDTO>>>
    {
        public string NameOrNumber { get; set; }
        public string BranchRegionId { get; set; }
        public string BankId { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }

    public class GerBranchHandler : IRequestHandler<GetBranchsQuery, ResultOperationDTO<PaginationDto<BranchDTO>>>
    {

        private readonly IBranchServices _branchServices;
        public GerBranchHandler(IBranchServices branchServices)
        {
            _branchServices = branchServices;
        }
        public async Task<ResultOperationDTO<PaginationDto<BranchDTO>>> Handle(GetBranchsQuery request, CancellationToken cancellationToken)
        {
            var result = await _branchServices.
                GetBranchs(request.NameOrNumber, request.BranchRegionId,
                            request.BankId, request.PageNo, request.PageSize);

            return ResultOperationDTO<PaginationDto<BranchDTO>>.CreateSuccsessOperation(result);
        }
    }
}
