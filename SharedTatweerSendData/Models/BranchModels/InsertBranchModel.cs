namespace SharedTatweerSendData.Models.BranchModels
{
    public class InsertBranchModel : BaseBranchModel
    {

        public string UserId { get; set; }
        public long LastSerialCertified { get; set; }
        public long LastSerial { get; set; }
        public long LastCountChekBook { get; set; }

        public InsertBranchSettingModel BranchSetting { get; set; }
        public List<BranchWorkTimeModel> BranchWorkTimes { get; set; }
    }
}
