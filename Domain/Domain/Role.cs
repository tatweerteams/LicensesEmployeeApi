using Infra;

namespace Domain.Domain
{
    public partial class Role
    {
        public Role()
        {
            RolePermisstions = new HashSet<RolePermisstion>();
            Users = new HashSet<User>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateAt { get; set; }
        public string UserId { get; set; }
        public DateTime? ModifyAt { get; set; }
        public virtual User CreateUser { get; set; }
        public virtual ICollection<RolePermisstion> RolePermisstions { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public UserTypeState UserType { get; set; }

    }
}
