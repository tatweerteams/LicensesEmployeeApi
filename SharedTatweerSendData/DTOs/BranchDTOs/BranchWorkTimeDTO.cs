namespace SharedTatweerSendData.DTOs.BranchDTOs
{
    public class BranchWorkTimeDTO
    {
        public string Id { get; set; }
        public DayOfWeek DayName { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public bool IsActive { get; set; }
    }
}
