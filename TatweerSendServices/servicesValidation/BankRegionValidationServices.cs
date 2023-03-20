using Infra;
using TatweerSendDomain.Domain;

namespace TatweerSendServices.servicesValidation
{
    public interface IBankRegionValidationServices
    {
        Task<bool> CanDeleteRegion(string regionId, CancellationToken cancellationToken = default);
        Task<bool> CanNotDeleteBank(string bankId, CancellationToken cancellationToken = default);
        Task<bool> ExistData(string bankId, string regionId, CancellationToken cancellationToken = default);
        Task<bool> CanDeleteBankRegion(string bankRegionId, CancellationToken cancellationToken = default);
        Task<bool> CheckExistBankRegion(string bankRegionId, CancellationToken cancellationToken = default);

    }

    public class BankRegionValidationServices : IBankRegionValidationServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public BankRegionValidationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CanNotDeleteBank(string bankId, CancellationToken cancellationToken = default)
        => await _unitOfWork.GetRepositoryReadOnly<BankRegion>().
            AnyAsync(pred => pred.BankId.Equals(bankId) && pred.Branchs.Any(), cancellationToken);

        public async Task<bool> CanDeleteRegion(string regionId, CancellationToken cancellationToken = default)
        => await _unitOfWork.GetRepositoryReadOnly<BankRegion>().
            AnyAsync(pred => pred.RegionId.Equals(regionId), cancellationToken);

        public async Task<bool> CanDeleteBankRegion(string bankRegionId, CancellationToken cancellationToken = default)
        => await _unitOfWork.GetRepositoryReadOnly<BankRegion>().
            AnyAsync(pred => pred.Id.Equals(bankRegionId) && pred.Branchs.Any(), cancellationToken);

        public async Task<bool> ExistData(string bankId, string regionId, CancellationToken cancellationToken = default)
        => await _unitOfWork.GetRepositoryReadOnly<BankRegion>().
            AnyAsync(pred => pred.RegionId.Equals(regionId) && pred.BankId.Equals(bankId), cancellationToken);

        public async Task<bool> CheckExistBankRegion(string bankRegionId, CancellationToken cancellationToken = default)
        => await _unitOfWork.GetRepositoryReadOnly<BankRegion>().
            AnyAsync(pred => pred.Id.Equals(bankRegionId), cancellationToken);

    }
}
