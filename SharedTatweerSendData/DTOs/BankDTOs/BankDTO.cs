namespace SharedTatweerSendData.DTOs
{
    public class BankDTO : ActiveBankDTO
    {
        public bool IsActive { get; set; }
        public List<BankRegionDTO> BankRegions { get; set; }
        public int BankRegionCount { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
