using Infra;
using MediatR;
using SharedTatweerSendData.Models.Accounts;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.AccountCommands;

public class InsertAccountsCommand : IRequest<ResultOperationDTO<bool>>
{
    public InsertAccountsModel insertAccountsModel { get; set; }
}

public class InsertAccountsHandler : IRequestHandler<InsertAccountsCommand, ResultOperationDTO<bool>>
{

    private readonly IAccountServices _accountServices;

    public InsertAccountsHandler(IAccountServices accountServices)
    {
        _accountServices = accountServices;
    }

    public async Task<ResultOperationDTO<bool>> Handle(InsertAccountsCommand request, CancellationToken cancellationToken)
    {
        await _accountServices.InsertListOfAccounts(request.insertAccountsModel.insertModel);
        return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { "تم العملية الإضافة بنجاح" });
    }
}

