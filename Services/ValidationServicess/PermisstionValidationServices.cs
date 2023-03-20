using Domain.Domain;
using Infra;

namespace IdentityServices.ValidationServicess
{
    public interface IPermisstionValidationServices
    {
        Task<bool> CheckName(string name);
        Task<bool> CheckName(string id, string name);

    }

    public class PermisstionValidationServices : IPermisstionValidationServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public PermisstionValidationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CheckName(string name)
         => await _unitOfWork.GetRepositoryReadOnly<Permisstion>().AnyAsync(pred => pred.Name.Equals(name));

        public async Task<bool> CheckName(string id, string name)
         => await _unitOfWork.GetRepositoryReadOnly<Permisstion>().
                        AnyAsync(pred => !pred.Id.Equals(id) && pred.Name.Equals(name));

    }
}
