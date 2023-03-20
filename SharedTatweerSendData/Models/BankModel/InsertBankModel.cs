namespace SharedTatweerSendData.Models
{
    public class InsertBankModel : BaseBankModel
    {
        public string UserId { get; set; }
        public List<BankRegionModel> BankRegions { get; set; }

    }
}
