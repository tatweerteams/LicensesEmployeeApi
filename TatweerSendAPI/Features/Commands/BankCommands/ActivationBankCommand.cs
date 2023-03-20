using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.BankCommands
{
    public class ActivationBankCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string BankId { get; set; }
        public bool IsActive { get; set; }

    }

    public class ActivationBankHandler : IRequestHandler<ActivationBankCommand, ResultOperationDTO<bool>>
    {
        private readonly IBankServices _bankServices;
        public ActivationBankHandler(IBankServices bankServices)
        {
            _bankServices = bankServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(ActivationBankCommand request, CancellationToken cancellationToken)
        {
            await _bankServices.ActivationBank(request.BankId, request.IsActive, cancellationToken);

            return ResultOperationDTO<bool>.
                CreateSuccsessOperation(true, message: new string[] { "تمت عملية بنجاح" });

        }
    }
}
