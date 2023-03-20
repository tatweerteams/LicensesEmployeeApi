namespace ReciveAPI.Domain
{
    public partial class Table
    {
        public int Id { get; set; }
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
        public bool? IsDone { get; set; }
        public int? Increment { get; set; }

        public DateTime? AothrizedOrdersDate { get; set; }
    }
}
