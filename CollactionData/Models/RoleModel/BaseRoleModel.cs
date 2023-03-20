using CollactionData.Models.RolePermisstionModel;
using Infra;

namespace CollactionData.Models.RoleModel
{
    public class BaseRoleModel
    {
        public string Name { get; set; }
        public UserTypeState UserType { get; set; }
        public virtual List<BaseRolePermisstionModel> RolePermisstions { get; set; }
    }
}
