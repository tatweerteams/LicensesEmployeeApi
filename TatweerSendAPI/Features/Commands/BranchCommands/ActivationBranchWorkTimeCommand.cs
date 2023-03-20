using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.BranchCommands
{
    public class ActivationBranchWorkTimeCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string BranchWorkTimeId { get; set; }
        public bool IsActive { get; set; }
    }

    public class ActivationBranchWorkTimeHandler : IRequestHandler<ActivationBranchWorkTimeCommand, ResultOperationDTO<bool>>
    {
        private readonly IBranchServices _branchServices;
        public ActivationBranchWorkTimeHandler(IBranchServices branchServices)
        {
            _branchServices = branchServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(ActivationBranchWorkTimeCommand request, CancellationToken cancellationToken)
        {
            await _branchServices.ActivationBranchWorkTime(request.BranchWorkTimeId, request.IsActive);

            var message = request.IsActive ? "تم عملية إلغاء تفعيل" : "تم عملية التفعيل";

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { message });
        }
    }
}
