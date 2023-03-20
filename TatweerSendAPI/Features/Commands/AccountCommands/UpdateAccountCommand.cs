using Infra;
using MediatR;
using SharedTatweerSendData.Models.Accounts;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.AccountCommands;

public class UpdateAccountCommand : IRequest<ResultOperationDTO<bool>>
{
    public UpdateAccountModel model { get; set; }

}

public class UpdateAccountHandler : IRequestHandler<UpdateAccountCommand, ResultOperationDTO<bool>>
{
    private readonly IAccountServices _accountServices;

    public UpdateAccountHandler(IAccountServices accountServices)
    {
        _accountServices = accountServices;
    }

    public async Task<ResultOperationDTO<bool>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        await _accountServices.UpdateAccount(request.model);
        return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { "تم عملية التعديل بنجاح" });
    }

}