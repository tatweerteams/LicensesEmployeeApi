using Infra;
using AutoMapper;
using TatweerSendDomain.Domain;
using SharedTatweerSendData.DTOs.AccountDTOs;
using TatweerSendServices.ExtensionServices;
using SharedTatweerSendData.Models.Accounts;
using FilterAttributeWebAPI.Common;

namespace TatweerSendServices.services;

public interface IAccountServices
{
    Task<PaginationDto<AccountDTO>> GetAccounts(string nameOrNumber, string branchRegionId, string bankId, string branchId, BaseAccountType accountType, int pageNo = 1, int pageSize = 30, CancellationToken cancellationToken = default);
    Task InsertAccount(InsertAccountModel accounts, CancellationToken cancellationToken = default);
    Task InsertListOfAccounts(List<InsertAccountModel> accounts);

    Task UpdateAccount(UpdateAccountModel accounts);
    Task DeleteAccount(string id, CancellationToken cancellationToken = default);
    Task ActivateAccount(string id, AccountState accountState, CancellationToken cancellationToken = default);
    Task ActivatePrintExternally(string id, bool status, CancellationToken cancellationToken = default);
}

public class AccountServices : IAccountServices
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AccountServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationDto<AccountDTO>> GetAccounts(string nameOrNumber, string branchRegionId, string bankId, string branchId, BaseAccountType accountType, int pageNo = 1, int pageSize = 30, CancellationToken cancellationToken = default)
    {

        var filterResult = (await _unitOfWork.GetRepositoryReadOnly<Account>().FindBy
            (predicate: nameOrNumber.SearchAccount( branchRegionId, bankId, branchId, accountType),
            selector: select => new AccountDTO
            {
                id = select.Id,
                accountNo = select.AccountNo,
                accountName = select.AccountName,
                bankName = select.Branch.BranchRegion.Bank.Name,
                bankId = select.Branch.BranchRegion.Bank.Id,
                regionName = select.Branch.BranchRegion.Region.Name,
                regionId = select.Branch.BranchRegion.Region.Id,
                branchId = select.Branch.Id,
                branchName = select.Branch.Name,
                branchNo = select.Branch.BranchNo,
                phoneNumber = select.PhoneNumber,
                bankRegionId = select.Branch.BranchRegionId,
                accountTypeValue = (int)select.AccountType,
                accountStateValue = (int)select.AccountState,
                printExternally = select.PrintExternally
            },
               pageNo: pageNo,
               pageSize: pageSize
            )).OrderBy(order => order.accountName).ToList();

        var recordsCount = await _unitOfWork.GetRepositoryReadOnly<Account>().GetCount(nameOrNumber.SearchAccount(branchRegionId, bankId, branchId, accountType));

        return new PaginationDto<AccountDTO>()
        {
            Data = filterResult,
            PageCount = recordsCount > 0 ? (int)Math.Ceiling(recordsCount / (double)pageSize) : 0
        };

    }

    public async Task InsertAccount(InsertAccountModel account, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.GetRepositoryWriteOnly<Account>().Insert(_mapper.Map<Account>(account), cancellationToken);
        await _unitOfWork.SaveChangeAsync();
    }
    public async Task InsertListOfAccounts(List<InsertAccountModel> accounts)
    {
        //var map = _mapper.Map<List<Account>>(accounts);
        await _unitOfWork.GetRepositoryWriteOnly<Account>().InsertList(_mapper.Map<List<Account>>(accounts));

        await _unitOfWork.SaveChangeAsync();
    }
    public async Task UpdateAccount(UpdateAccountModel account)
    {
        var oldData = await _unitOfWork.GetRepositoryReadOnly<Account>().GetByID(account.Id);
        if (oldData == null) throw new ApplicationEx("بيانات الحساب غير متوفرة");

        await _unitOfWork.GetRepositoryWriteOnly<Account>().Update(_mapper.Map(account, oldData));
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task ActivateAccount(string id, AccountState accountState, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.GetRepositoryReadOnly<Account>().GetByID(id);
        if (result == null) throw new ApplicationEx("بيانات الحساب غير متوفرة");

        result.AccountState = accountState.ChangeAccountStateExtenstion();
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task DeleteAccount(string id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.GetRepositoryReadOnly<Account>().GetByID(id);
        if (result == null) throw new ApplicationEx("بيانات الحساب غير متوفرة");

        await _unitOfWork.GetRepositoryWriteOnly<Account>().Remove(result);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }

    public async Task ActivatePrintExternally(string id, bool staus, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.GetRepositoryReadOnly<Account>().GetByID(id);
        if (result == null) throw new ApplicationEx("بيانات الحساب غير متوفرة");

        result.PrintExternally = !staus;
        await _unitOfWork.GetRepositoryWriteOnly<Account>().Update(result);
        await _unitOfWork.SaveChangeAsync();
    }

}
