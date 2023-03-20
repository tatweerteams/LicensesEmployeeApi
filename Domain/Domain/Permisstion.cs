namespace Domain.Domain
{
    public partial class Permisstion
    {
        public Permisstion()
        {
            RolePermisstions = new HashSet<RolePermisstion>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<RolePermisstion> RolePermisstions { get; set; }
    }
}
