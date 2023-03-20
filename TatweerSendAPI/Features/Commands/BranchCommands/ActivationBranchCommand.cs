using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.BranchCommands
{
    public class ActivationBranchCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string BranchId { get; set; }
        public bool IsActive { get; set; }
    }

    public class ActivationBranchHandler : IRequestHandler<ActivationBranchCommand, ResultOperationDTO<bool>>
    {
        private readonly IBranchServices _branchServices;
        public ActivationBranchHandler(IBranchServices branchServices)
        {
            _branchServices = branchServices;
        }

        public async Task<ResultOperationDTO<bool>> Handle(ActivationBranchCommand request, CancellationToken cancellationToken)
        {
            await _branchServices.ActivationBranch(request.BranchId, request.IsActive);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { "تم عملية بنجاح" });
        }
    }
}
