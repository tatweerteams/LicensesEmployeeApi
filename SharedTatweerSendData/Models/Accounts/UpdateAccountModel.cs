using Infra;

namespace SharedTatweerSendData.Models.Accounts;

public class UpdateAccountModel : baseAccountModel
{
    public string UserId { get; set; }
    public string Id { get; set; }
    //public bool PrintExternally { get; set; } = false;
    public BaseAccountType AccountType { get; set; }
}
