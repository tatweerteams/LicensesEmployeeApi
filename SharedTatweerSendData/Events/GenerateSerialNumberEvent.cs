namespace Events
{
    public class GenerateSerialNumberEvent
    {
        public string RequestIdentity { get; set; }
        public bool PrintOutCenter { get; set; } = false;
    }
}
