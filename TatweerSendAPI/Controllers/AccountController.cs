using Infra;
using Infra.Utili;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedTatweerSendData.DTOs.AccountDTOs;
using SharedTatweerSendData.Models.Accounts;
using TatweerSendAPI.Features.Commands.AccountCommands;
using TatweerSendAPI.Features.Queries.AccountQueries;
using TatweerSendAPI.Filters.AccountFilter;

namespace TatweerSendAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
public class AccountController : BaseController
{

    private readonly IMediator _mediator;
    private readonly HelperUtili _helper;

    public AccountController(IMediator mediator, HelperUtili helper)
    {
        _mediator = mediator;
        _helper = helper;
    }

    [Authorize(Roles = RolesUtili.AddOrUpdateAccounts + "," + RolesUtili.Administrator + "," + RolesUtili.AddAccountsToOtherBranches + "," + RolesUtili.DislpayAccountsOfOtherBranches)]
    [HttpGet("GetAccounts")]
    public async Task<ResultOperationDTO<PaginationDto<AccountDTO>>>
    GetAccounts(string nameOrNumber, string bankId, string bankRegionId, string branchId, int accountType, int pageNo = 1, int pageSize = 30)
        => await _mediator.Send(new GetAccountsListQuery
        {
            NameOrNumber = nameOrNumber,
            BankId = bankId ?? _helper.GetCurrentUser()?.BankId,
            branchId = branchId ?? _helper.GetCurrentUser()?.BranchId,
            AccountType = accountType,
            branchRegionId = bankRegionId,
            PageNo = pageNo,
            PageSize = pageSize
        });

    [Authorize(Roles = RolesUtili.AddOrUpdateAccounts + "," + RolesUtili.Administrator + "," + RolesUtili.AddAccountsToOtherBranches)]
    [HttpPost("InsertAccount")]
    [TypeFilter(typeof(InsertAccountFilter))]
    public async Task<ResultOperationDTO<bool>> InsertAccount([FromBody] InsertAccountModel model, CancellationToken cancellationToken = default)
        => await _mediator.Send(new InsertAccountCommand { insertAccountModel = model });

    [Authorize(Roles = RolesUtili.AddOrUpdateAccounts + "," + RolesUtili.Administrator + "," + RolesUtili.AddAccountsToOtherBranches)]
    [HttpPost("InsertListOfAccounts")]
    [TypeFilter(typeof(InsertListOfAccountsFilter))]
    public async Task<ResultOperationDTO<bool>> InsertAccounts([FromBody] InsertAccountsModel model, CancellationToken cancellationToken = default)
        => await _mediator.Send(new InsertAccountsCommand { insertAccountsModel = model });

    [Authorize(Roles = RolesUtili.AddOrUpdateAccounts + "," + RolesUtili.Administrator + "," + RolesUtili.AddAccountsToOtherBranches)]
    [HttpPost("UpdateAccount")]
    [TypeFilter(typeof(UpdateAccountFilter))]
    public async Task<ResultOperationDTO<bool>> UpdateAccount([FromBody] UpdateAccountModel model, CancellationToken cancellationToken = default)
        => await _mediator.Send(new UpdateAccountCommand { model = model });

    [Authorize]
    [HttpDelete("DeleteAccount")]
    [TypeFilter(typeof(DeleteAccountFilter))]
    public async Task<ResultOperationDTO<bool>> DeleteAccount(string id, CancellationToken cancellationToken = default)
        => await _mediator.Send(new DeleteAccountCommand { id = id });

    [Authorize]
    [HttpPut("ActivateAccount")]
    public async Task<ResultOperationDTO<bool>> ActivateAccount(string id, AccountState accountState, CancellationToken cancellationToken = default)
        => await _mediator.Send(new ActivateAccountCommand { id = id, AccountState = accountState });

    [Authorize]
    [HttpPut("ActivatePrintExternally")]
    public async Task<ResultOperationDTO<bool>> ActivatePrintExternally(string id, bool status, CancellationToken cancellationToken = default)
        => await _mediator.Send(new ActivatePrintExternallyCommand { id = id, status = status });
}
