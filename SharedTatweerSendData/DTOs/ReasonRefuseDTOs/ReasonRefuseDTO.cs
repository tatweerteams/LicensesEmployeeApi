namespace SharedTatweerSendData.DTOs.ReasonRefuseDTOs
{
    public class ReasonRefuseDTO : ActiveReasonRefuseDTO
    {
        public string CreateAt { get; set; }
        public string EmployeeNo { get; set; }
        public bool IsActive { get; set; }
    }
    public class ActiveReasonRefuseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
