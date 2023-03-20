using Infra;

namespace TatweerSendDomain.Domain
{
    public class Account : BaseDomain
    {

        public Account()
        {
            OrderItems = new HashSet<OrderItem>();
        }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string BranchId { get; set; }
        public string PhoneNumber { get; set; }
        public BaseAccountType AccountType { get; set; }
        public AccountState AccountState { get; set; }
        public InputTypeState InputType { get; set; } = InputTypeState.Defualt;
        public virtual Branch Branch { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public bool PrintExternally { get; set; } = false;

    }
}
