using Infra;
using LogginDomain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LogginServices.Services.ExtensionServices
{
    public static class LogginDataExtensionServices
    {
        public static Expression<Func<LogginData, bool>> SearchLogginEventExpression(this string userName, string branchNo, string branchName, EventTypeState? eventType,
            UserTypeState? userType, DateTime? from, DateTime? to)
            => pred =>
                    (string.IsNullOrWhiteSpace(userName) || pred.UserName.Contains(userName)) &&
                    (string.IsNullOrWhiteSpace(branchNo) || pred.BranchNumber.Contains(branchNo)) &&
                    (string.IsNullOrWhiteSpace(branchName) || pred.BranchNumber.Contains(branchName)) &&
                    (eventType == null || pred.EventType.Equals(eventType)) &&
                    (userType == null || pred.UserType.Equals(userType)) &&
                    (from.HasValue && to.HasValue ?
                      EF.Functions.DateDiffDay(from, pred.CreateAt) >= 0 &&
                      EF.Functions.DateDiffDay(to, pred.CreateAt) <= 0 : true);
    }
}
