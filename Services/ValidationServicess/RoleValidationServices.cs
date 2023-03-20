using Domain.Domain;
using Infra;

namespace IdentityServices.ValidationServicess
{
    public interface IRoleValidationServices
    {
        Task<bool> CheckIsExists(string roleId);
        Task<bool> CheckNameExists(string name);
        Task<bool> CheckNameExists(string roleId, string name);
        Task<bool> CheckCanDelete(string roleId);

    }
    public class RoleValidationServices : IRoleValidationServices
    {

        private readonly IUnitOfWork _unitOfWork;
        public RoleValidationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CheckCanDelete(string roleId)
            => await _unitOfWork.GetRepositoryReadOnly<Role>().
                                AnyAsync(pred => pred.Id.Equals(roleId) && pred.Users.Any());

        public async Task<bool> CheckIsExists(string roleId)
            => await _unitOfWork.GetRepositoryReadOnly<Role>().
                                AnyAsync(pred => pred.Id.Equals(roleId));

        public async Task<bool> CheckNameExists(string name)
            => await _unitOfWork.GetRepositoryReadOnly<Role>().
                                AnyAsync(pred => pred.Name.Equals(name));

        public async Task<bool> CheckNameExists(string roleId, string name)
            => await _unitOfWork.GetRepositoryReadOnly<Role>().
                                AnyAsync(pred => !pred.Id.Equals(roleId) && pred.Name.Equals(name));
    }
}
