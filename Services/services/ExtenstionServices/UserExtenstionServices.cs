using Domain;
using Infra;
using System.Linq.Expressions;

namespace IdentityServices.services.ExtenstionServices
{
    public static class UserExtenstionServices
    {
        public static Expression<Func<User, bool>> SearchUserExpression(this string branchId,
        UserTypeState? userType, string name, string regionId, string employeeNo, string phoneNumber)

           => pred => (string.IsNullOrEmpty(branchId) || pred.BranchId.Contains(branchId)) &&
                                (string.IsNullOrEmpty(name) || (pred.Name.Contains(name) || pred.Email.Contains(name))) &&
                                (string.IsNullOrEmpty(employeeNo) || pred.EmployeeNumber.Contains(employeeNo)) &&
                                (string.IsNullOrEmpty(regionId) || pred.RegionId.Contains(regionId)) &&
                                (string.IsNullOrEmpty(phoneNumber) || pred.PhoneNumber.Contains(phoneNumber)) &&
                                (userType == null || pred.Role.UserType.Equals(userType));
    }
}
