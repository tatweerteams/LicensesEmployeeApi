using Infra;
using TatweerSendDomain.Domain;

namespace TatweerSendServices.servicesValidation
{
    public interface IBranchValidationServices
    {
        Task<bool> IsExistsData(string name, string number, string branchRegionId);
        Task<List<string>> IsExistsData(List<string> name, List<string> number, string branchRegionId);
        Task<bool> IsExistsData(string id, string name, string number, string branchRegionId);
        Task<bool> CheckIsExistBranchId(string id);
        Task<bool> CanNotDeleteBranch(string id);
        Task<bool> CheckBranchSettingIsExist(string branchSettingId);
        Task<bool> CheckBranchWorkTimeExists(string branchWorkId);
    }

    public class BranchValidationServices : IBranchValidationServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public BranchValidationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CanNotDeleteBranch(string id)
        => (await _unitOfWork.GetRepositoryReadOnly<Branch>().AnyAsync(pred =>
                pred.Id.Equals(id) &&
                (pred.Accounts.Any() || pred.OrderRequests.Any())
             ));

        public async Task<bool> CheckBranchSettingIsExist(string branchSettingId)
        => await _unitOfWork.GetRepositoryReadOnly<BranchSetting>().AnyAsync(pred => pred.Id.Equals(branchSettingId));

        public async Task<bool> CheckBranchWorkTimeExists(string branchWorkId)
        => await _unitOfWork.GetRepositoryReadOnly<BranchWorkTime>().AnyAsync(pred => pred.Id.Equals(branchWorkId));

        public async Task<bool> CheckIsExistBranchId(string id)
        => await _unitOfWork.GetRepositoryReadOnly<Branch>().AnyAsync(pred => pred.Id.Equals(id));

        public async Task<bool> IsExistsData(string name, string number, string branchRegionId)
        => await _unitOfWork.GetRepositoryReadOnly<Branch>().AnyAsync(pred =>
            pred.BranchRegionId.Equals(branchRegionId) &&
            (pred.Name.Contains(name) || pred.BranchNo.Contains(number))
        );

        public async Task<bool> IsExistsData(string id, string name, string number, string branchRegionId)
         => await _unitOfWork.GetRepositoryReadOnly<Branch>().AnyAsync(pred =>
            !pred.Id.Equals(id) &&
            pred.BranchRegionId.Equals(branchRegionId) &&
           (pred.Name.Contains(name) || pred.BranchNo.Contains(number))
        );

        public async Task<List<string>> IsExistsData(List<string> name, List<string> number, string branchRegionId)
        {
            var result = (await _unitOfWork.GetRepositoryReadOnly<Branch>().
                FindBy(
                 predicate: pred =>
                    pred.BranchRegionId.Equals(branchRegionId) &&
                  (name.Contains(pred.Name) || number.Contains(pred.BranchNo)),
                 selector: select => select.BranchNo
                )).ToList();

            return result;
        }
    }
}
