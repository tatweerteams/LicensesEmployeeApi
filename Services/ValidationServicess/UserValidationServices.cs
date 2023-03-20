using Domain;
using Infra;

namespace IdentityServices.ValidationServicess
{
    public interface IUserValidationServices
    {
        Task<bool> CheckUserExists(string employeeNo, string phoneNo, string email);
        Task<bool> CheckUserExists(string id, string employeeNo, string phoneNo, string email);
        Task<bool> CheckIsExists(string id);

    }
    public class UserValidationServices : IUserValidationServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserValidationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CheckIsExists(string id)
            => await _unitOfWork.GetRepositoryReadOnly<User>().AnyAsync(pred => pred.Id.Equals(id));

        public async Task<bool> CheckUserExists(string employeeNo, string phoneNo, string email)
            => await _unitOfWork.GetRepositoryReadOnly<User>().AnyAsync(pred =>
                pred.EmployeeNumber.Equals(employeeNo) || pred.PhoneNumber.Equals(phoneNo) || pred.Email.Equals(email));

        public async Task<bool> CheckUserExists(string id, string employeeNo, string phoneNo, string email)
            => await _unitOfWork.GetRepositoryReadOnly<User>().AnyAsync(pred => !pred.Id.Equals(id) &&
                (pred.EmployeeNumber.Equals(employeeNo) || pred.PhoneNumber.Equals(phoneNo) || pred.Email.Equals(email)));
    }
}
