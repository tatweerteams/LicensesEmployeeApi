using Infra;
using MediatR;
using SharedTatweerSendData.Models;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.BankRegionCommands
{
    public class InsertBankRegionCommand : IRequest<ResultOperationDTO<bool>>
    {
        public InsertBankRegionModel model { get; set; }
    }

    public class InsertBankRegionHandler : IRequestHandler<InsertBankRegionCommand, ResultOperationDTO<bool>>
    {

        private readonly IBankRegionServices _bankRegionServices;
        public InsertBankRegionHandler(IBankRegionServices bankRegionServices)
        {
            _bankRegionServices = bankRegionServices;
        }

        public async Task<ResultOperationDTO<bool>> Handle(InsertBankRegionCommand request, CancellationToken cancellationToken)
        {
            await _bankRegionServices.AddBankRegion(request.model, cancellationToken);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "تمت عملية إضافة بنجاح" });
        }
    }
}
