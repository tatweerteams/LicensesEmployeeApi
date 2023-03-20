using Infra;
using MediatR;
using SharedTatweerSendData.Models;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.BankCommands
{
    public class InsertBankCommand : IRequest<ResultOperationDTO<bool>>
    {
        public InsertBankModel BankModel { get; set; }
    }

    public class InsertBankHandler : IRequestHandler<InsertBankCommand, ResultOperationDTO<bool>>
    {
        private readonly IBankServices _bankServices;
        public InsertBankHandler(IBankServices bankServices)
        {
            _bankServices = bankServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(InsertBankCommand request, CancellationToken cancellationToken)
        {
            await _bankServices.InsertBank(request.BankModel);
            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { "تم العملية الإضافة بنجاح" });
        }
    }
}
