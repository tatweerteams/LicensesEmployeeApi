namespace SharedTatweerSendData.DTOs
{
    public class BankRegionDTO : BankRegionActiveDTO
    {
        public bool IsActive { get; set; }
        public string BankId { get; set; }
        public string BankName { get; set; }
        public string BankNo { get; set; }
        public int BranchCount { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
