using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.BankRegionCommands
{
    public class DeleteBankRegionCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string BankRegionId { get; set; }
    }

    public class DeleteBankRegionHandler : IRequestHandler<DeleteBankRegionCommand, ResultOperationDTO<bool>>
    {
        private readonly IBankRegionServices _bankRegionServices;
        public DeleteBankRegionHandler(IBankRegionServices bankRegionServices)
        {
            _bankRegionServices = bankRegionServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(DeleteBankRegionCommand request, CancellationToken cancellationToken)
        {
            await _bankRegionServices.DeleteBankRegion(request.BankRegionId, cancellationToken);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "تم إلغاء المنطقة بنجاح" });

        }
    }
}
