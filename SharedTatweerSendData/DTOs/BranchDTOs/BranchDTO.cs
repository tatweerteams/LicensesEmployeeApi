namespace SharedTatweerSendData.DTOs
{
    public class BranchDTO : ActiveBranchDTO
    {
        public bool IsActive { get; set; }
        public long LastSerialCertified { get; set; }
        public long LastSerial { get; set; }
        public long LastCountChekBook { get; set; }
        public string BranchRegionId { get; set; }
        public string BranchRegionName { get; set; }
        public string BankName { get; set; }
        public string BankId { get; set; }
        public int AccountCount { get; set; }
        public int OrderRequestCount { get; set; }
        public string BranchSettingId { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
