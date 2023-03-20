namespace SharedTatweerSendData.DTOs.ReportDTOs
{
    public class StatisticBranchDTO
    {
        public string BranchName { get; set; }
        public string BranchNumber { get; set; }
        public string RegionName { get; set; }
        public int OrderRequestCount { get; set; }
        public int OrderRequestCompanyCount { get; set; }
        public int AccountCount { get; set; }
        public int OrderRequestIndividualCount { get; set; }

    }
}
