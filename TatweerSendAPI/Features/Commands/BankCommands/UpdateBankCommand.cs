using Infra;
using MediatR;
using SharedTatweerSendData.Models;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.BankCommands
{
    public class UpdateBankCommand : IRequest<ResultOperationDTO<bool>>
    {
        public UpdateBankModel BankModel { get; set; }
    }

    public class UpdateBankHandler : IRequestHandler<UpdateBankCommand, ResultOperationDTO<bool>>
    {
        private readonly IBankServices _bankServices;
        public UpdateBankHandler(IBankServices bankServices)
        {
            _bankServices = bankServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(UpdateBankCommand request, CancellationToken cancellationToken)
        {
            await _bankServices.UpdateBank(request.BankModel, cancellationToken);
            return ResultOperationDTO<bool>.
                CreateSuccsessOperation(true, message: new string[] { "تم عملية تعديل البيانات المصرف" });
        }
    }
}
