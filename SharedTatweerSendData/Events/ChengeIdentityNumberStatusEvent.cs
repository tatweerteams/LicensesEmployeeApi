using Infra;

namespace Events
{
    public class ChengeIdentityNumberStatusEvent
    {
        public List<string> IdentityNumbers { get; set; }
        public OrderRequestState OrderRequestState { get; set; }
    }


}
