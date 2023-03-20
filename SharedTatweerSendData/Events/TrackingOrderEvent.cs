using Infra;

namespace SharedTatweerSendData.Events
{
    public class TrackingOrderEvent
    {
        public string OrderRequestId { get; set; }
        public DateTime OrderCreationDate { get; set; }
        public string UserId { get; set; }
        public string EmployeeNo { get; set; }
        public UserTypeState UserType { get; set; }
        public string RejectNote { get; set; }
        public OrderRequestState OrderRequestState { get; set; }
    }
}
