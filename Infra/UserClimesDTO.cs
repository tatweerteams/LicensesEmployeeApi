

namespace Infra
{
    public class UserClimesDTO
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string BranchNumber { get; set; }
        public UserTypeState? UserType { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string RegionId { get; set; }
        public string RegionName { get; set; }
        public string BankId { get; set; }
        public string BankName { get; set; }
        public string RoleName { get; internal set; }
        public string EmployeeNumber { get; internal set; }
    }
}
