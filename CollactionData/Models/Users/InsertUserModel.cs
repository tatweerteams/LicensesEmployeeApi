namespace CollactionData.Models.Users
{
    public class InsertUserModel : BaseUserModel
    {
        public string PasswordHash { get; set; }
        public bool SendPassword { get; set; }

    }
}
