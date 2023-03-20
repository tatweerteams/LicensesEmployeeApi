using Infra;

namespace SharedTatweerSendData.DTOs
{
    public class BaseOrderRequestDTO
    {
        public string Id { get; set; }
        public string IdentityNumber { get; set; }
        public BaseAccountType OrderRequestType { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public OrderRequestState OrderRequestState { get; set; }
        public int MinOrderItemBranch { get; set; }
        public int AccountCount { get; set; }
    }
    public class OrderRequestDTO : BaseOrderRequestDTO
    {
        public string Note { get; set; }
        public string IdentityNumberBank { get; set; }
        public string RequestDate { get; set; }
        public string EmployeeNo { get; set; }
        public string BranchRegionId { get; set; }
        public string RegionName { get; set; }
        public DateTime CreateAt { get; set; }
        public int AccountSuspendedCount { get; set; }
    }

    public class RejectNoteDTO
    {
        public string Note { get; set; }
        public string CreateAtString { get; set; }
        public string EmpolyeeNo { get; set; }
    }


}
