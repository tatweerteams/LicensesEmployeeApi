using Infra;
using TatweerSendDomain.Domain;

namespace TatweerSendServices.servicesValidation
{
    public interface IReasonRefuseValidationServices
    {
        Task<bool> CheckIsExists(int id);
        Task<bool> CheckMessageExests(string name);
        Task<bool> CheckMessageExests(int id, string name);
    }

    public class ReasonRefuseValidationServices : IReasonRefuseValidationServices
    {

        public readonly IUnitOfWork _unitOfWork;
        public ReasonRefuseValidationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckIsExists(int id)
        => await _unitOfWork.GetRepositoryReadOnly<ReasonRefuse>().AnyAsync(
                pred => pred.Id.Equals(id)
            );

        public async Task<bool> CheckMessageExests(string name)
            => await _unitOfWork.GetRepositoryReadOnly<ReasonRefuse>().AnyAsync(
                    pred => pred.Name.Equals(name));

        public async Task<bool> CheckMessageExests(int id, string name)
            => await _unitOfWork.GetRepositoryReadOnly<ReasonRefuse>().AnyAsync(
                    pred => !pred.Id.Equals(id) && pred.Name.Equals(name));
    }
}
