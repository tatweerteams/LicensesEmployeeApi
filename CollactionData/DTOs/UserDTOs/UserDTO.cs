using Infra;

namespace CollactionData.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public bool SendSMS { get; set; }
        public string PhoneNumber { get; set; }
        public UserTypeState UserType { get; set; }
        public string BranchNumber { get; set; }
        public DateTime CreateAt { get; set; }
        public string EmployeeNumber { get; set; }
        public string BranchName { get; set; }
        public string BranchId { get; set; }
        public string RegionName { get; set; }
        public string RegionId { get; set; }
        public string BankName { get; set; }
        public string BankId { get; set; }
    }
}
