namespace TatweerSendDomain.Domain
{
    public class Bank : BaseDomain
    {
        public Bank()
        {
            BankRegions = new HashSet<BankRegion>();
        }
        public string Name { get; set; }
        public string BankNo { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<BankRegion> BankRegions { get; set; }
    }
}
