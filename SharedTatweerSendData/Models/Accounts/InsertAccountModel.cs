using Infra;

namespace SharedTatweerSendData.Models.Accounts
{
    public class InsertAccountModel : baseAccountModel
    {
        public string UserId { get; set; }

        public BaseAccountType AccountType { get; set; }
        public AccountState AccountState { get; set; }
        public InputAccountTypeState InputType { get; set; } = InputAccountTypeState.Defualt;
    }
}
