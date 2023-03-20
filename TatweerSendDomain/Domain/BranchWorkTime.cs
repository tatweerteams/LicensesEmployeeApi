namespace TatweerSendDomain.Domain
{
    public class BranchWorkTime
    {

        public string Id { get; set; }
        public DateTime? ModifyAt { get; set; }
        public DayOfWeek DayName { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string BranchId { get; set; }
        public bool IsActive { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
