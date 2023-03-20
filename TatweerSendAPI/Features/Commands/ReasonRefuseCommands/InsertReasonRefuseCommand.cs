using Infra;
using MediatR;
using SharedTatweerSendData.Models.ReasonRefuseModel;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.ReasonRefuseCommands
{
    public class InsertReasonRefuseCommand : IRequest<ResultOperationDTO<bool>>
    {
        public InsertReasonRefuseModel Model { get; set; }
    }

    public class InsertReasonRefuseHandler : IRequestHandler<InsertReasonRefuseCommand, ResultOperationDTO<bool>>
    {
        private readonly IReasonRefuseServices _reasonRefuseServices;
        public InsertReasonRefuseHandler(IReasonRefuseServices reasonRefuseServices)
        {
            _reasonRefuseServices = reasonRefuseServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(InsertReasonRefuseCommand request, CancellationToken cancellationToken)
        {
            await _reasonRefuseServices.InsertReasonRefuse(request.Model);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { "تم إضافة سبب الرفض" });
        }
    }
}
