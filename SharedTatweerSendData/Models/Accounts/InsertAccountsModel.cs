namespace SharedTatweerSendData.Models.Accounts;

public class InsertAccountsModel 
{
    public string branchId { get; set; }
    public List<InsertAccountModel> insertModel { get; set; }
    public List<InsertAccountModel> updateModel { get; set; }

}
