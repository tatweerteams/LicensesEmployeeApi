using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.BranchCommands
{
    public class DeleteBranchCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string BranchId { get; set; }
    }

    public class DeleteBranchHandler : IRequestHandler<DeleteBranchCommand, ResultOperationDTO<bool>>
    {
        private readonly IBranchServices _branchServices;
        public DeleteBranchHandler(IBranchServices branchServices)
        {
            _branchServices = branchServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
        {
            await _branchServices.DeleteBranch(request.BranchId);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { "تم عملية إلغاء الفرع" });
        }
    }
}
