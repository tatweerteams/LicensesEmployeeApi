using Infra;

namespace CollactionData.Models.AuthUserModel
{
    public class LoginUserModel
    {
        public string NameOrNumber { get; set; }
        public string Password { get; set; }

        public UserAuthDTO UserAuth = new();
    }

    public class ChengePasswordModel
    {
        public string EmployeeNumber { get; set; }
        public string Password { get; set; }
        public string UserId { get; set; }

    }
}
