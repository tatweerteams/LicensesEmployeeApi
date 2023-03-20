namespace TatweerSendDomain.Domain
{
    public class ReasonRefuse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? ModifyAt { get; set; }
        public string EmployeeNo { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }

    }
}
