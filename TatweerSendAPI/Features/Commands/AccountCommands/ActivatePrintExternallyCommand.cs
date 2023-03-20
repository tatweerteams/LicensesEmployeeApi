using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.AccountCommands;

public class ActivatePrintExternallyCommand : IRequest<ResultOperationDTO<bool>>
{
    public string id { get; set; }
    public bool status { get; set; }
}

public class ActivatePrintExternallyHandler : IRequestHandler<ActivatePrintExternallyCommand, ResultOperationDTO<bool>>
{
    
    private readonly IAccountServices _accountServices;

    public ActivatePrintExternallyHandler(IAccountServices accountServices)
    {
        _accountServices = accountServices;
    }

    public async Task<ResultOperationDTO<bool>> Handle(ActivatePrintExternallyCommand request, CancellationToken cancellationToken)
    {
        await _accountServices.ActivatePrintExternally(request.id, request.status);
        return ResultOperationDTO<bool>.CreateSuccsessOperation(true, message: new string[] { "تمت العملية التعديل بنجاح" });
    }

}
