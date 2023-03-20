using Infra;

namespace TatweerSendDomain.Domain
{
    public class OrderItem
    {
        public string Id { get; set; }
        public string OrderRequestId { get; set; }
        public string AccountId { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string SerialFrom { get; set; }
        public int CountChekBook { get; set; }
        public OrderItemState State { get; set; } = OrderItemState.Success;
        public DateTime CreateAt { get; set; }
        public virtual Account Account { get; set; }
        public virtual OrderRequest OrderRequest { get; set; }

    }
}
