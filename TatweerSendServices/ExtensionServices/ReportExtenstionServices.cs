using Infra;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TatweerSendDomain.Domain;

namespace TatweerSendServices.ExtensionServices
{
    public static class ReportExtenstionServices
    {
        public static Expression<Func<OrderItem, bool>> SearchAccountRequestExpression(this string accountNo, string branchId, BaseAccountType? accountType,
            string phoneNo, DateTime? from, DateTime? to)
           => pred =>
                    pred.AccountNo.EndsWith(accountNo) &&
                    pred.OrderRequest.BranchId.Equals(branchId) &&
                    (string.IsNullOrWhiteSpace(phoneNo) || pred.Account.PhoneNumber.Contains(phoneNo)) &&
                    (accountType == null || pred.Account.AccountType.Equals(accountType));

        //&&
        //         (from.HasValue && to.HasValue?
        //             EF.Functions.DateDiffDay(from, pred.OrderRequest.CreateAt) >= 0 &&
        //             EF.Functions.DateDiffDay(to, pred.OrderRequest.CreateAt) <= 0 : true

        public static Expression<Func<OrderEvent, bool>> SearchEmpolyeeReportExpression(this string branchId, string employeeNo, DateTime? from, DateTime? to)
         => pred =>
                 pred.OrderRequest.BranchId.Equals(branchId) &&
                (string.IsNullOrWhiteSpace(employeeNo) || pred.EmployeeNo.Equals(employeeNo)) &&
                (from.HasValue && to.HasValue ?
                            EF.Functions.DateDiffDay(from, pred.OrderRequest.CreateAt) >= 0 &&
                            EF.Functions.DateDiffDay(to, pred.OrderRequest.CreateAt) <= 0 : true);

        public static Expression<Func<OrderRequest, bool>> SearchBranchOrderReportExpression(this string branchId,
             string identityNo, OrderRequestState? orderRequestState, BaseAccountType? orderRequestType,
             InputTypeState? inputType, DateTime? from, DateTime? to, bool printOutCenter = false, string fromSerial = null, string toSerial = null)
         => pred =>
                    pred.BranchId.Equals(branchId) &&
                    pred.PrintOutCenter == printOutCenter &&
                    (string.IsNullOrWhiteSpace(identityNo) || (pred.IdentityNumber.Equals(identityNo) || pred.IdentityNumberBank.Equals(identityNo))) &&
                    (orderRequestState == null || pred.OrderRequestState.Equals(orderRequestState)) &&
                    (orderRequestType == null || pred.OrderRequestType.Equals(orderRequestType)) &&
                    (inputType == null || pred.InputTypeState.Equals(inputType)) &&
                    (from.HasValue && to.HasValue ?
                                (EF.Functions.DateDiffDay(from, pred.CreateAt) >= 0 &&
                                EF.Functions.DateDiffDay(to, pred.CreateAt) <= 0) : true);
    }
}
