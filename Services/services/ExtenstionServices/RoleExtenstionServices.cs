using Domain.Domain;
using Infra;
using System.Linq.Expressions;

namespace IdentityServices.services.ExtenstionServices
{
    public static class RoleExtenstionServices
    {
        public static Expression<Func<Role, bool>> SearchRoleExpression(this string name,
         UserTypeState? userType)
            => pred =>
                        (string.IsNullOrWhiteSpace(name) || pred.Name.Contains(name)) &&
                        (userType == null || pred.UserType.Equals(userType));
    }
}
