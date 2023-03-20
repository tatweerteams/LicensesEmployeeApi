namespace TatweerSendDomain.Domain
{
    public class BankRegion : BaseDomain
    {
        public BankRegion()
        {
            Branchs = new HashSet<Branch>();
        }
        public string BankId { get; set; }
        public string RegionId { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Branch> Branchs { get; set; }
        public virtual Bank Bank { get; set; }
        public virtual Region Region { get; set; }
    }
}
