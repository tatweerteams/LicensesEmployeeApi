namespace TatweerSendDomain.Domain
{
    public class BranchSetting
    {
        public string Id { get; set; }
        public DateTime? ModifyAt { get; set; }
        public string AccountChekBook { get; set; }
        public int CompanyFrom { get; set; }
        public int CompanyTo { get; set; }
        public int CertifiedFrom { get; set; }
        public int CertifiedTo { get; set; }
        public int IndividualFrom { get; set; }
        public int IndividualTo { get; set; }
        public string BranchId { get; set; }
        public bool IndividualRequestAccountDay { get; set; } // رقم الحساب في الطلبية لمدة يوم مايقدر يطلب خاص بالافراد
        public int IndividualQuentityOfDay { get; set; } = 5; // عدد الدفاتر التي سيتم طلبها في الطلبية لكل حساب خاص بالافراد
        public int BranchRequestCountOfDay { get; set; } = 10;
        public bool OrderRequestAuthorization { get; set; } = true;
        public virtual Branch Branch { get; set; }
    }
}
