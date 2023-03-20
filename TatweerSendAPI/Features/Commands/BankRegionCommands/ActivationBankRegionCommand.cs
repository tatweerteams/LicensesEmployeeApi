using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.BankRegionCommands
{
    public class ActivationBankRegionCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string BankRegionId { get; set; }
        public bool IsActive { get; set; }
    }

    public class ActivationBankRegionHandler : IRequestHandler<ActivationBankRegionCommand, ResultOperationDTO<bool>>
    {
        private readonly IBankRegionServices _bankRegionServices;
        public ActivationBankRegionHandler(IBankRegionServices bankRegionServices)
        {
            _bankRegionServices = bankRegionServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(ActivationBankRegionCommand request, CancellationToken cancellationToken)
        {
            await _bankRegionServices.Activation(request.BankRegionId, request.IsActive, cancellationToken);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "تمت عملية بنحاح" });
        }
    }
}
