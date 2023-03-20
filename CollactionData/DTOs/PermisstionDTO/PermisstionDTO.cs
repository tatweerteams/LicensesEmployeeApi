namespace CollactionData.DTOs
{
    public class PermisstionDTO : ActivePermisstionDTO
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
    public class ActivePermisstionDTO
    {
        public string Id { get; set; }
        public string Description { get; set; }
    }
}
