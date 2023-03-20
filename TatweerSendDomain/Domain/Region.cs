namespace TatweerSendDomain.Domain
{
    public class Region : BaseDomain
    {
        public Region()
        {
            BankRegions = new HashSet<BankRegion>();
        }
        public string Name { get; set; }
        public string RegionNo { get; set; }
        public virtual ICollection<BankRegion> BankRegions { get; set; }
    }
}
