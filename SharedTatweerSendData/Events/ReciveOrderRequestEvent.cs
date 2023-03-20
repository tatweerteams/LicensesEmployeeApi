namespace Events
{
    public class ReciveOrderRequestEvent
    {
        public List<OrderRequestItemEvent> OrderItems { get; set; }
    }
    public class OrderRequestItemEvent
    {
        public string ForCount { get; set; }
        public string RequestIdentity { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string Tc { get; set; }
        public int? ChCount { get; set; }
        public DateTime? RequestDate { get; set; }
        public string FromSerial { get; set; }
        public string BranchName { get; set; }
        public string MyUser { get; set; }
        public string BranchNumber { get; set; }
        public string RegName { get; set; }
        public string RegionNumber { get; set; }
        public int? RequestStatus { get; set; }
        public bool? IsDone { get; set; } = false;
    }
}
