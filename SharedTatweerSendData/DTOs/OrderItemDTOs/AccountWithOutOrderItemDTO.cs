namespace SharedTatweerSendData.DTOs
{
    public class AccountWithOutOrderItemDTO
    {
        public string AccountId { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public bool? IndividualRequestAccountDay { get; set; }
        public bool IsInsert { get; set; }
        public int CountChekBook { get; set; }
        public int? RequestQuantity { get; set; }
    }
}
