using Infra;

namespace Events
{
    public class GenereteIdentityNoEvent
    {
        public string OrderRequestId { get; set; }
        public string EmployeeNo { get; set; }
        public string UserId { get; set; }
        public UserTypeState UserType { get; set; }
        public InputTypeState InputType { get; set; }

    }
}
