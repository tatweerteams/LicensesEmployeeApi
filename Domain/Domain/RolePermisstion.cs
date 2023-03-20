namespace Domain.Domain
{
    public partial class RolePermisstion
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string PermisstionId { get; set; }

        public virtual Permisstion Permisstion { get; set; }
        public virtual Role Role { get; set; }
    }
}
