using Infra;

namespace SharedTatweerSendData.DTOs.ReportDTOs
{
    public class AccountRequestReportDTO
    {
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string RequestDate { get; set; }
        public DateTime RequestAt { get; set; }
        public string BranchName { get; set; }
        public string IdentityNumber { get; set; }
        public OrderItemState OrderItemState { get; set; }
        public OrderRequestState OrderRequestState { get; set; }
        public string OrderRequestNote { get; set; }
    }

    public class EmployeeReportDTO
    {
        public string RequestDate { get; set; }
        public DateTime RequestAt { get; set; }
        public string BranchName { get; set; }
        public string IdentityNumber { get; set; }
        public OrderRequestState OrderRequestState { get; set; }
        public string OrderRequestNote { get; set; }
        public string OrderCreationDate { get; set; }
        public string EmployeeNo { get; set; }
    }

    public class BranchOrderReportDTO
    {
        public string OrderRequestId { get; set; }
        public string IdentityNumber { get; set; }
        public string RequestDate { get; set; }
        public DateTime RequestAt { get; set; }
        public string BranchName { get; set; }
        public string BranchNumber { get; set; }
        public OrderRequestState OrderRequestState { get; set; }
        public BaseAccountType OrderRequestType { get; set; }
        public string OrderRequestNote { get; set; }
        public InputTypeState InputType { get; set; }
        public string EmployeeNo { get; set; }
        public int CountChekBook { get; set; }
    }
    public class OrderRequestPriteOutDTO : BranchOrderReportDTO
    {
        public string FromSerial { get; set; }
        public string ToSerial { get; set; }

    }


}
