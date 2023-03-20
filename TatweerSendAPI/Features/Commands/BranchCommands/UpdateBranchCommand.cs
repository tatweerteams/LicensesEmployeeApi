using Infra;
using MediatR;
using SharedTatweerSendData.Models.BranchModels;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.BranchCommands
{
    public class UpdateBranchCommand : IRequest<ResultOperationDTO<bool>>
    {
        public UpdateBranchModel BranchModel { get; set; }
    }

    public class UpdateBranchHandler : IRequestHandler<UpdateBranchCommand, ResultOperationDTO<bool>>
    {

        private readonly IBranchServices _branchServices;
        public UpdateBranchHandler(IBranchServices branchServices)
        {
            _branchServices = branchServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
        {
            await _branchServices.UpdateBranch(request.BranchModel);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { "تم عملية تعديل بنجاح" });
        }
    }
}
