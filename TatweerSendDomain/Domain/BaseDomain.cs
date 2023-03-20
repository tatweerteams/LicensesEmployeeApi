namespace TatweerSendDomain.Domain
{
    public class BaseDomain
    {
        public string Id { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime? ModifyAt { get; set; }
        public string UserId { get; set; }
    }
}
