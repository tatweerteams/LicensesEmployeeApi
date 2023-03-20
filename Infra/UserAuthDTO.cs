
namespace Infra
{
    public class UserAuthDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<string> Permisstions { get; set; }
        public string AccessToken { get; set; }
        public string PhoneNumber { get; set; }
        public string BranchNumber { get; set; }
        public string BranchName { get; set; }
        public string RegionName { get; set; }
        public string RegionId { get; set; }
        public string BankName { get; set; }
        public string BankId { get; set; }
        public string BranchId { get; set; }
        public UserTypeState UserTypeState { get; set; }
        public CheckIsAdminState CheckIsAdminState { get; set; } = CheckIsAdminState.IsNotAdmin;
        public string EmployeeNumber { get; set; }
        public bool IsActive { get; set; }
        public bool? IsFirstLogin { get; set; }
    }
}
