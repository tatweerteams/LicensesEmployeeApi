namespace TatweerSendDomain.Domain
{
    public class Branch : BaseDomain
    {
        public Branch()
        {
            BranchWorkTimes = new HashSet<BranchWorkTime>();
            OrderRequests = new HashSet<OrderRequest>();
            Accounts = new HashSet<Account>();
        }

        public string Name { get; set; }
        public string BranchNo { get; set; }
        public bool IsActive { get; set; }
        public long LastSerialCertified { get; set; }
        public long LastSerial { get; set; }
        public long LastCountChekBook { get; set; }
        public string BranchRegionId { get; set; }
        public virtual BankRegion BranchRegion { get; set; }
        public virtual BranchSetting BranchSetting { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<BranchWorkTime> BranchWorkTimes { get; set; }
        public virtual ICollection<OrderRequest> OrderRequests { get; set; }


    }
}
