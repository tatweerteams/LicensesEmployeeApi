using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.ReasonRefuseCommands
{
    public class DeleteReasonRefuseCommand : IRequest<ResultOperationDTO<bool>>
    {
        public int ReasonRefuseId { get; set; }
    }

    public class DeleteReasonRefuseHandler : IRequestHandler<DeleteReasonRefuseCommand, ResultOperationDTO<bool>>
    {
        private readonly IReasonRefuseServices _reasonRefuseServices;
        public DeleteReasonRefuseHandler(IReasonRefuseServices reasonRefuseServices)
        {
            _reasonRefuseServices = reasonRefuseServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(DeleteReasonRefuseCommand request, CancellationToken cancellationToken)
        {
            await _reasonRefuseServices.DeleteReasonRefuse(request.ReasonRefuseId);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true,
                message: new string[] { "تم عملية إلغاء بيانات سبب الرفض" });
        }
    }
}
