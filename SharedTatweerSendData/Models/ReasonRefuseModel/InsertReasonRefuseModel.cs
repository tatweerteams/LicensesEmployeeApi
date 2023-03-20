namespace SharedTatweerSendData.Models.ReasonRefuseModel
{
    public class InsertReasonRefuseModel : BaseReasonRefuseModel
    {
        public string EmployeeNo { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateReasonRefuseModel : BaseReasonRefuseModel
    {
        public int Id { get; set; }

    }
}
