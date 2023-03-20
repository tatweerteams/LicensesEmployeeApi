using Infra;
using MediatR;
using SharedTatweerSendData.Models.BranchModels;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.BranchCommands
{
    public class UpdateBranchSettingCommand : IRequest<ResultOperationDTO<bool>>
    {
        public UpdateBranchSettingModel BranchSetting { get; set; }
    }

    public class UpdateBranchSettingHandler : IRequestHandler<UpdateBranchSettingCommand, ResultOperationDTO<bool>>
    {
        private readonly IBranchServices _branchServices;
        public UpdateBranchSettingHandler(IBranchServices branchServices)
        {
            _branchServices = branchServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(UpdateBranchSettingCommand request, CancellationToken cancellationToken)
        {
            await _branchServices.UpdateBranchSetting(request.BranchSetting);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { "تم عملية التعديل الإعدادات" });
        }
    }
}
