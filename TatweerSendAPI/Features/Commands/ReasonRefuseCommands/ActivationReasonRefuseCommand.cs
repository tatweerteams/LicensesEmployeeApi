using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.ReasonRefuseCommands
{
    public class ActivationReasonRefuseCommand : IRequest<ResultOperationDTO<bool>>
    {
        public int ReasonRefuseId { get; set; }
        public bool IsActive { get; set; }
    }

    public class ActivationReasonRefuseHandler : IRequestHandler<ActivationReasonRefuseCommand, ResultOperationDTO<bool>>
    {
        private readonly IReasonRefuseServices _reasonRefuseServices;
        public ActivationReasonRefuseHandler(IReasonRefuseServices reasonRefuseServices)
        {
            _reasonRefuseServices = reasonRefuseServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(ActivationReasonRefuseCommand request, CancellationToken cancellationToken)
        {
            await _reasonRefuseServices.ActivationReasonRefuse(request.ReasonRefuseId, request.IsActive);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true,
                message: new string[] { "تم عملية بنجاح" });
        }
    }
}
