namespace SharedTatweerSendData.Models.BranchModels
{
    public class InsertBranchCollectionModel
    {
        public string BranchRegionId { get; set; }
        public List<ImportBranchList> Branchs { get; set; }

        public List<ImportBranchList> ExistBranchs = new();
    }

    public class ImportBranchList : InsertBranchModel
    {
        public string Note { get; set; }
    }
}
