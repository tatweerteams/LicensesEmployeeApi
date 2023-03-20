using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.AccountCommands
{
    public class DeleteAccountCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string id { get; set; }
    }

    public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand, ResultOperationDTO<bool>>
    {

        private readonly IAccountServices _accountServices;

        public DeleteAccountHandler(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        public async Task<ResultOperationDTO<bool>> Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
        {
            await _accountServices.DeleteAccount(command.id, cancellationToken);
            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "تم إلغاء الحساب بنجاح" });
        }

    }
}
