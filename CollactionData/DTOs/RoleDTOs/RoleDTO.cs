using Infra;

namespace CollactionData.DTOs
{
    public class RoleDTO : ActiveRoleDTO
    {

        public UserTypeState UserType { get; set; }
        public List<ActivePermisstionDTO> RolePermisstions { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateAt { get; set; }
        public string UserName { get; set; }
    }

    public class ActiveRoleDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
