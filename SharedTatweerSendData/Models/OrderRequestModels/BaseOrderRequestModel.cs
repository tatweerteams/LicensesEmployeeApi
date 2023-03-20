using Infra;

namespace SharedTatweerSendData.Models.OrderRequestModels
{
    public class BaseOrderRequestModel
    {

        public string Note { get; set; }
        public BaseAccountType OrderRequestType { get; set; }
        public string BranchId { get; set; }
        public bool PrintOutCenter { get; set; } = false;

    }
}
