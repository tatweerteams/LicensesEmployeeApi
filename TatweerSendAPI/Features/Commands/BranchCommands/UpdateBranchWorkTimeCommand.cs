using Infra;
using MediatR;
using SharedTatweerSendData.Models.BranchModels;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.BranchCommands
{
    public class UpdateBranchWorkTimeCommand : IRequest<ResultOperationDTO<bool>>
    {
        public BranchWorkTimeModel BranchWorkTime { get; set; }
    }

    public class UpdateBranchWorkTimeHandler : IRequestHandler<UpdateBranchWorkTimeCommand, ResultOperationDTO<bool>>
    {
        private readonly IBranchServices _branchServices;
        public UpdateBranchWorkTimeHandler(IBranchServices branchServices)
        {
            _branchServices = branchServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(UpdateBranchWorkTimeCommand request, CancellationToken cancellationToken)
        {
            await _branchServices.UpdateBranchWorkTime(request.BranchWorkTime);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { "تم تعديل التوقيت الفرع" });
        }
    }
}
