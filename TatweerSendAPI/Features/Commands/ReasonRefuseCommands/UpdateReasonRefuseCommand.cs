using Infra;
using MediatR;
using SharedTatweerSendData.Models.ReasonRefuseModel;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.ReasonRefuseCommands
{
    public class UpdateReasonRefuseCommand : IRequest<ResultOperationDTO<bool>>
    {
        public UpdateReasonRefuseModel Model { get; set; }
    }

    public class UpdateReasonRefuseHandler : IRequestHandler<UpdateReasonRefuseCommand, ResultOperationDTO<bool>>
    {
        private readonly IReasonRefuseServices _reasonRefuseServices;
        public UpdateReasonRefuseHandler(IReasonRefuseServices reasonRefuseServices)
        {
            _reasonRefuseServices = reasonRefuseServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(UpdateReasonRefuseCommand request, CancellationToken cancellationToken)
        {
            await _reasonRefuseServices.UpdateReasonRefuse(request.Model);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true,
                message: new string[] { "تم عملية تعديل بيانات سبب الرفض" });
        }
    }
}
