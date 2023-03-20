using Infra;

namespace SharedTatweerSendData.Events
{
    public class LogginDataEvent
    {
        public string Id { get; set; }
        public string Messages { get; set; }
        public string NewData { get; set; }
        public string OldData { get; set; }
        public EventTypeState EventType { get; set; }
        public UserTypeState UserType { get; set; }
        public string ConnectionId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string BranchNumber { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
