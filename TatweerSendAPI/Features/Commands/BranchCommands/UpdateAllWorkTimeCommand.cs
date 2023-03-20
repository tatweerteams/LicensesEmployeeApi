using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.BranchCommands
{
    public class UpdateAllWorkTimeCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string TimeStrart { get; set; }
        public string TimeEnd { get; set; }
        public List<DayOfWeek> Days { get; set; }
        public bool IsActive { get; set; }
    }
    public class UpdateAllWorkTimeHandler : IRequestHandler<UpdateAllWorkTimeCommand, ResultOperationDTO<bool>>
    {
        private readonly IBranchServices _branchServices;
        public UpdateAllWorkTimeHandler(IBranchServices branchServices)
        {
            _branchServices = branchServices;
        }

        public async Task<ResultOperationDTO<bool>> Handle(UpdateAllWorkTimeCommand request, CancellationToken cancellationToken)
        {
            await _branchServices.UpdateAllWorkTime(request.TimeStrart, request.TimeEnd, request.Days, request.IsActive);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "تم تعديل التوقيت بنجاح" });

        }
    }
}
