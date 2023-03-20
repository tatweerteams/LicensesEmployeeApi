using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.AccountCommands;

public class ActivateAccountCommand : IRequest<ResultOperationDTO<bool>>
{
    public string id { get; set; }
    public AccountState AccountState { get; set; }
}

public class InsertAccountHandler : IRequestHandler<ActivateAccountCommand, ResultOperationDTO<bool>>
{

    private readonly IAccountServices _accountServices;

    public InsertAccountHandler(IAccountServices accountServices)
    {
        _accountServices = accountServices;
    }

    public async Task<ResultOperationDTO<bool>> Handle(ActivateAccountCommand request, CancellationToken cancellationToken)
    {
        await _accountServices.ActivateAccount(request.id, request.AccountState);
        return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { "تمت العملية التعديل بنجاح" });
    }
}
