using Infra;

namespace SharedTatweerSendData.DTOs
{
    public class OrderItemResultDTO
    {
        public BaseOrderRequestDTO OrderRequest { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
        public int PageCount { get; set; }
    }

    public class OrderItemDTO
    {
        public string Id { get; set; }
        public string OrderRequestId { get; set; }
        public string AccountId { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string SerialFrom { get; set; }
        public int CountChekBook { get; set; }
        public OrderItemState State { get; set; }
        public string CreateAt { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
