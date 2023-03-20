using Infra;

namespace SharedTatweerSendData.Models.OrderRequestModels
{
    public class InsertOrderRequestModel : BaseOrderRequestModel
    {
        public OrderRequestState OrderRequestState { get; set; } = OrderRequestState.Process;

        public string EmployeeNo { get; set; }
        public string UserId { get; set; }

    }
}
