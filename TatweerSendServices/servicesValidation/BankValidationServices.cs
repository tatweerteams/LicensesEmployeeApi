using Infra;
using TatweerSendDomain.Domain;

namespace TatweerSendServices.servicesValidation
{
    public interface IBankValidationServices
    {
        Task<bool> CheckBankDataExists(string bankName, string bankNo, CancellationToken cancellationToken = default);
        Task<bool> CheckBankDataExists(string bankId, string bankName, string bankNo, CancellationToken cancellationToken = default);
        Task<bool> CheckBankIsNotActive(string bankId, CancellationToken cancellationToken = default);
        Task<bool> CheckBankIsExists(string bankId, CancellationToken cancellationToken = default);
    }

    public class BankValidationServices : IBankValidationServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public BankValidationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CheckBankDataExists(string bankName, string bankNo, CancellationToken cancellationToken = default)
        => await _unitOfWork.GetRepositoryReadOnly<Bank>().AnyAsync(pred =>
            pred.Name.Equals(bankName) || pred.BankNo.Equals(bankNo)
            , cancellationToken);

        public async Task<bool> CheckBankDataExists(string bankId, string bankName, string bankNo, CancellationToken cancellationToken = default)
         => await _unitOfWork.GetRepositoryReadOnly<Bank>().AnyAsync(pred =>
            !pred.Id.Equals(bankId) && (pred.Name.Equals(bankName) || pred.BankNo.Equals(bankNo))
            , cancellationToken);

        public async Task<bool> CheckBankIsExists(string bankId, CancellationToken cancellationToken = default)
         => await _unitOfWork.GetRepositoryReadOnly<Bank>().AnyAsync(pred =>
            pred.Id.Equals(bankId), cancellationToken);

        public async Task<bool> CheckBankIsNotActive(string bankId, CancellationToken cancellationToken = default)
        => await _unitOfWork.GetRepositoryReadOnly<Bank>().AnyAsync(pred =>
            pred.Id.Equals(bankId) && pred.IsActive == false, cancellationToken);
    }
}
