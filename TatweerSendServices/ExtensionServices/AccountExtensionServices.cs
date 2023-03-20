using FilterAttributeWebAPI.Common;
using Infra;
using System.Linq.Expressions;
using TatweerSendDomain.Domain;

namespace TatweerSendServices.ExtensionServices
{
    public static class AccountExtensionServices
    {

        public static Expression<Func<Account, bool>> SearchAccount(this string nameOrNumber, string branchRegionId, string bankId, string branchId, BaseAccountType accountType)
            =>  pred => ((string.IsNullOrWhiteSpace(nameOrNumber) || (pred.AccountNo.Contains(nameOrNumber) ||
                pred.AccountName.Contains(nameOrNumber))) &&
                ( pred.Branch.BranchRegion.BankId.Equals(bankId) ) &&
                (string.IsNullOrWhiteSpace(branchRegionId) || pred.Branch.BranchRegionId.Equals(branchRegionId)) &&
                (accountType == 0 || pred.AccountType.Equals(accountType) ) &&
                (string.IsNullOrWhiteSpace(branchId) || pred.BranchId.Equals(branchId)));

        public static AccountState ChangeAccountStateExtenstion(this AccountState accountState)
           => accountState switch
           {
               AccountState.IsActive => AccountState.IsSuspended,
               AccountState.IsSuspended => AccountState.IsActive,
               _ => throw new ApplicationEx("يجب مراجعة مسؤول علي النظام")
           };

    }
}
