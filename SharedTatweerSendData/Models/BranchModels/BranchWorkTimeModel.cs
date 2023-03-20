namespace SharedTatweerSendData.Models.BranchModels
{
    public class BranchWorkTimeModel
    {
        public string Id { get; set; }
        public DayOfWeek DayName { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
    }




}
