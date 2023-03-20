using Infra;
using TatweerSendDomain.Domain;

namespace TatweerSendServices.servicesValidation
{
    public interface IAccountValidationServices
    {
        Task<bool> IsAccountNumberExist(string accountNo, CancellationToken cancellationToken = default);
        Task<bool> IsPhoneNumberExist(string phoneNumber, CancellationToken cancellationToken = default);
        Task<bool> IsAccountExistGetById(string id, CancellationToken cancellationToken = default);
        Task<bool> IsAccountBelongToAnotherPerson(string id, string branchId, string newAccountNo, CancellationToken cancellationToken = default);
        Task<bool> IsValidAccountInputToUpdate(string id, string branchId, string newAccountNo, CancellationToken cancellationToken = default);
        Task<bool> IsAccountHasAnyOrder(string id, CancellationToken cancellationToken = default);
        Task<Branch> GetBranchById(string id);
        Task<List<string>> GetListOfAccounts(List<string> accountNos, string branchId, CancellationToken cancellationToken = default);


    }

    public class AccountValidationServices : IAccountValidationServices
    {

        private readonly IUnitOfWork _unitOfWork;

        public AccountValidationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<string>> GetListOfAccounts(List<string> accountNos, string branchId, CancellationToken cancellationToken = default)
            => await _unitOfWork.GetRepositoryReadOnly<Account>().FindBy(pred => pred.BranchId.Equals(branchId) && accountNos.Contains(pred.AccountNo),
            selector: select => select.AccountNo
        );


        public async Task<bool> IsAccountNumberExist(string accountNo, CancellationToken cancellationToken = default)
            => await _unitOfWork.GetRepositoryReadOnly<Account>().AnyAsync(pred =>
            pred.AccountNo.Equals(accountNo), cancellationToken);

        public async Task<bool> IsPhoneNumberExist(string phoneNumber, CancellationToken cancellationToken = default)
            => await _unitOfWork.GetRepositoryReadOnly<Account>().AnyAsync(pred =>
            pred.PhoneNumber.Equals(phoneNumber), cancellationToken);

        public async Task<Branch> GetBranchById(string id)
            => await _unitOfWork.GetRepositoryReadOnly<Branch>().GetByID(id);

        public async Task<bool> IsAccountHasAnyOrder(string id, CancellationToken cancellationToken = default)
           => await _unitOfWork.GetRepositoryReadOnly<OrderItem>().AnyAsync(pred =>
                pred.Account.Id.Equals(id), cancellationToken);

        public async Task<bool> IsAccountExistGetById(string id, CancellationToken cancellationToken = default)
            => await _unitOfWork.GetRepositoryReadOnly<Account>().AnyAsync(pred =>
            pred.Id.Equals(id), cancellationToken);

        public async Task<bool> IsAccountBelongToAnotherPerson(string id, string branchId, string newAccountNo, CancellationToken cancellationToken = default)
            => await _unitOfWork.GetRepositoryReadOnly<Account>().AnyAsync(pred =>
            !pred.Id.Equals(id) && pred.BranchId.Equals(branchId) && pred.AccountNo == newAccountNo, cancellationToken);

        public async Task<bool> IsValidAccountInputToUpdate(string id, string branchId, string newAccountNo, CancellationToken cancellationToken = default)
        {
            var branch = await _unitOfWork.GetRepositoryReadOnly<Branch>().GetByID(branchId);
            if (branch == null) return false;

            if    (newAccountNo.Substring(0, 3).Contains(branch.BranchNo) == false)  return false;
            else  return true;
        }

    }
}
