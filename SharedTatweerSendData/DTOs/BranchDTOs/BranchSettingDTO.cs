namespace SharedTatweerSendData.DTOs
{
    public class BranchSettingDTO
    {
        public string Id { get; set; }
        public string AccountChekBook { get; set; }
        public int CompanyFrom { get; set; }
        public int CompanyTo { get; set; }
        public int CertifiedFrom { get; set; }
        public int CertifiedTo { get; set; }
        public int IndividualFrom { get; set; }
        public int IndividualTo { get; set; }
        public bool IndividualRequestAccountDay { get; set; }
        public bool OrderRequestAuthorization { get; set; }
        public int IndividualQuentityOfDay { get; set; }
        public int BranchRequestCountOfDay { get; set; }
    }
}
