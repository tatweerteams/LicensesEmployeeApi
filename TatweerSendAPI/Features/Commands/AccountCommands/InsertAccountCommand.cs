using Infra;
using MediatR;
using SharedTatweerSendData.Models.Accounts;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.AccountCommands;

public class InsertAccountCommand : IRequest<ResultOperationDTO<bool>>
{
    public InsertAccountModel insertAccountModel { get; set; }
}

public class InsertBankHandler : IRequestHandler<InsertAccountCommand, ResultOperationDTO<bool>>
{

    private readonly IAccountServices _accountServices;

    public InsertBankHandler(IAccountServices accountServices)
    {
        _accountServices = accountServices;
    }

    public async Task<ResultOperationDTO<bool>> Handle(InsertAccountCommand request, CancellationToken cancellationToken)
    {
        await _accountServices.InsertAccount(request.insertAccountModel, cancellationToken);
        return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { "تم العملية الإضافة بنجاح" });
    }
}

