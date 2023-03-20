using Infra;

namespace TatweerSendDomain.Domain
{
    public class OrderEvent
    {
        public string Id { get; set; }
        public string OrderRequestId { get; set; }
        public DateTime OrderCreationDate { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        public string EmployeeNo { get; set; }
        public UserTypeState UserType { get; set; }
        public virtual OrderRequest OrderRequest { get; set; }
        public string RejectNote { get; set; }
        public OrderRequestState OrderRequestState { get; set; }

    }
}
