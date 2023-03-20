using Infra;
using MediatR;
using SharedTatweerSendData.Models.BranchModels;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.BranchCommands
{
    public class InsertBranchCommand : IRequest<ResultOperationDTO<bool>>
    {
        public InsertBranchModel BranchModel { get; set; }
    }

    public class InsertBankHandler : IRequestHandler<InsertBranchCommand, ResultOperationDTO<bool>>
    {
        private readonly IBranchServices _branchServices;
        public InsertBankHandler(IBranchServices branchServices)
        {
            _branchServices = branchServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(InsertBranchCommand request, CancellationToken cancellationToken)
        {
            await _branchServices.InsertBranch(request.BranchModel);
            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { "تم العملية الإضافة بنجاح" });
        }
    }

}
