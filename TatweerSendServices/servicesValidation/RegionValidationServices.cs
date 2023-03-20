using Infra;
using TatweerSendDomain.Domain;

namespace TatweerSendServices.servicesValidation
{
    public interface IRegionValidationServices
    {
        Task<bool> CheckRegionDataExists(string regionName, string regionNo, CancellationToken cancellationToken = default);
        Task<bool> CheckRegionDataExists(string regionId, string regionName, string regionNo, CancellationToken cancellationToken = default);
        Task<bool> CheckRegionDelete(string regionId, CancellationToken cancellationToken = default);

    }

    public class RegionValidationServices : IRegionValidationServices
    {

        private readonly IUnitOfWork _unitOfWork;
        public RegionValidationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CheckRegionDelete(string regionId, CancellationToken cancellationToken = default)
        => await _unitOfWork.GetRepositoryReadOnly<Region>().AnyAsync(pred => pred.Id.Equals(regionId), cancellationToken);

        public async Task<bool> CheckRegionDataExists(string regionName, string regionNo, CancellationToken cancellationToken = default)
         => await _unitOfWork.GetRepositoryReadOnly<Region>().
            AnyAsync(pred => pred.Name.Equals(regionName) || pred.RegionNo.Equals(regionNo), cancellationToken);

        public async Task<bool> CheckRegionDataExists(string regionId, string regionName, string regionNo, CancellationToken cancellationToken = default)
        => await _unitOfWork.GetRepositoryReadOnly<Region>().
             AnyAsync(pred => !pred.Id.Equals(regionId) && (pred.Name.Equals(regionName) || pred.RegionNo.Equals(regionNo)), cancellationToken);



    }
}
