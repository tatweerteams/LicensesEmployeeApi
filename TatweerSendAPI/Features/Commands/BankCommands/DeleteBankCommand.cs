using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.BankCommands
{
    public class DeleteBankCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string BankId { get; set; }

    }

    public class DeleteBankHandler : IRequestHandler<DeleteBankCommand, ResultOperationDTO<bool>>
    {
        private readonly IBankServices _bankServices;
        public DeleteBankHandler(IBankServices bankServices)
        {
            _bankServices = bankServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(DeleteBankCommand request, CancellationToken cancellationToken)
        {
            await _bankServices.DeleteBank(request.BankId, cancellationToken);

            return ResultOperationDTO<bool>.
                CreateSuccsessOperation(true, message: new string[] { "تمت عملية بنجاح" });

        }
    }
}
