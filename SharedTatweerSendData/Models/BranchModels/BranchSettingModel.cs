namespace SharedTatweerSendData.Models.BranchModels
{
    public class BaseBranchSettingModel
    {

        public string AccountChekBook { get; set; }
        public int CompanyFrom { get; set; }
        public int CompanyTo { get; set; }
        public int CertifiedFrom { get; set; }
        public int CertifiedTo { get; set; }
        public int IndividualFrom { get; set; }
        public int IndividualTo { get; set; }

        public bool IndividualRequestAccountDay { get; set; }
        public bool OrderRequestAuthorization { get; set; }
        public int IndividualQuentityOfDay { get; set; } = 5;
        public int BranchRequestCountOfDay { get; set; } = 10;
    }

    public class InsertBranchSettingModel : BaseBranchSettingModel
    {
        public string UserId { get; set; }

    }

    public class UpdateBranchSettingModel : BaseBranchSettingModel
    {
        public string Id { get; set; }
    }
}
