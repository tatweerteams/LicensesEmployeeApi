using Infra;

namespace CollactionData.DTOs
{
    public class UserSystemDTO
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Permisstion { get; set; }
        public List<string> Permisstions { get; set; }
        public string AccessToken { get; set; }
        public string ModuleName { get; set; }
        public string RoleName { get; set; }
        public string Id { get; set; }
        public CheckIsAdminState CheckIsAdminState { get; set; }
    }
}
