using FilterAttributeWebAPI.Common;
using Infra;
using MediatR;
using SharedTatweerSendData.DTOs.AccountDTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.AccountQueries;

public class GetAccountsListQuery : IRequest<ResultOperationDTO<PaginationDto<AccountDTO>>>
{
    public string NameOrNumber { get; set; }
    public string BankId { get; set; }
    public int AccountType { get; set; }
    public string branchRegionId { get; set; }
    public string branchId { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
}

public class GetAccountsListHandler : IRequestHandler<GetAccountsListQuery, ResultOperationDTO<PaginationDto<AccountDTO>>>
{
    private readonly IAccountServices _accountServices;

    public GetAccountsListHandler(IAccountServices accountServices)
    {
        _accountServices = accountServices;
    }

    public async Task<ResultOperationDTO<PaginationDto<AccountDTO>>> Handle(GetAccountsListQuery request, CancellationToken cancellationToken)
    {
        if (request.AccountType != 0)
            if (Enum.IsDefined(typeof(BaseAccountType), request.AccountType) == false)
                throw new ApplicationEx("يوجد خطأ في نوع الحساب");

        var result = await _accountServices.
            GetAccounts(
                nameOrNumber: request.NameOrNumber,
                branchRegionId: request.branchRegionId,
                bankId: request.BankId,
                accountType: (BaseAccountType)request.AccountType,
                branchId: request.branchId,
                pageNo: request.PageNo,
                pageSize: request.PageSize
            );

        return ResultOperationDTO<PaginationDto<AccountDTO>>.CreateSuccsessOperation(result);
    }
}
