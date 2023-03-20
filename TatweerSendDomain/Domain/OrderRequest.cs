using Infra;

namespace TatweerSendDomain.Domain
{
    public class OrderRequest
    {
        public OrderRequest()
        {
            OrderItems = new HashSet<OrderItem>();
            OrderEvents = new HashSet<OrderEvent>();

        }
        public string Id { get; set; }
        public string IdentityNumber { get; set; }
        public string Note { get; set; }
        public string IdentityNumberBank { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public OrderRequestState OrderRequestState { get; set; }
        public BaseAccountType OrderRequestType { get; set; }
        public InputTypeState InputTypeState { get; set; }
        public string UserId { get; set; }
        public string EmployeeNo { get; set; }
        public string BranchId { get; set; }
        public bool PrintOutCenter { get; set; } = false;
        public long ChCount { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual ICollection<OrderEvent> OrderEvents { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

    }
}
