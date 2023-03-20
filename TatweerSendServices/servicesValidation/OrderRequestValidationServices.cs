using Infra;
using Microsoft.EntityFrameworkCore;
using TatweerSendDomain.Domain;
using TatweerSendServices.ExtensionServices;

namespace TatweerSendServices.servicesValidation
{
    public interface IOrderRequestValidationServices
    {
        Task<bool> CheckOrderRequestExists(string id);
        Task<bool> CheckRequestOrder(string userId, string branchId = null, string orderRequestId = null);
        Task<bool> CheckBranchCanRequestOrder(string branchId);
        Task<bool> CheckRequestOrderItems(string id);
        Task<bool> CheckCountRequestOrderByBranchId(string branchId);

        Task<int?> CheckMinOrderItemInRequest(string orderRequestId);

        Task<bool> CheckCanApprovedRequest(string orderRequestId);
        Task<bool> CheckCanRejectRequest(string orderRequestId);

    }

    public class OrderRequestValidationServices : IOrderRequestValidationServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderRequestValidationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CheckRequestOrder(string userId, string branchId, string orderRequestId)
            => await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().
                AnyAsync(pred =>
                pred.UserId.Equals(userId) &&
                (string.IsNullOrEmpty(branchId) || pred.BranchId.Equals(branchId)) &&
                (string.IsNullOrEmpty(orderRequestId) || pred.Id.Equals(orderRequestId)) &&
                pred.OrderRequestState.Equals(OrderRequestState.Process));

        public async Task<bool> CheckRequestOrderItems(string id)
            => await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().
                AnyAsync(pred => pred.Id.Equals(id) && pred.OrderItems.Any());

        public async Task<bool> CheckBranchCanRequestOrder(string branchId)
            => await _unitOfWork.GetRepositoryReadOnly<Branch>().
                AnyAsync(pred =>
                pred.Id.Equals(branchId) &&
                pred.IsActive == true &&
                pred.BranchRegion.IsActive == true &&
                pred.BranchRegion.Bank.IsActive == true
                );


        public async Task<bool> CheckOrderRequestExists(string id)
            => await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().
                AnyAsync(pred => pred.Id.Equals(id));

        public async Task<bool> CheckCountRequestOrderByBranchId(string branchId)
            => await _unitOfWork.GetRepositoryReadOnly<Branch>().
                AnyAsync(pred => pred.Id.Equals(branchId) &&
                pred.OrderRequests.Count(c =>
                        //!c.InputTypeState.Equals(InputTypeState.Cosher) &&
                        !c.OrderRequestState.Equals(OrderRequestState.Process) &&
                    EF.Functions.DateDiffDay(c.CreateAt, DateTime.Now) == 0) >= pred.BranchSetting.BranchRequestCountOfDay);

        public async Task<int?> CheckMinOrderItemInRequest(string orderRequestId)
            => await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().SingalOfDefultAsync(
                    predicate: pred =>
                    pred.Id.Equals(orderRequestId) &&
                    pred.OrderItems.Count() < pred.Branch.BranchSetting.IndividualFrom,

                    selector: select => select.OrderRequestType.MinItemInRequestOrderExtensionValidation(
                                select.Branch.BranchSetting.IndividualFrom,
                                select.Branch.BranchSetting.CompanyFrom,
                                select.Branch.BranchSetting.CertifiedFrom)
                );

        public async Task<bool> CheckCanApprovedRequest(string orderRequestId)
            => await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().AnyAsync(pred =>
                        pred.Id.Equals(orderRequestId) &&
                        pred.OrderItems.Any(a => a.State.Equals(OrderItemState.Success)) &&
                        pred.OrderRequestState.Equals(OrderRequestState.Pinding)
                    );



        public async Task<bool> CheckCanRejectRequest(string orderRequestId)
            => await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().AnyAsync(pred =>
                    pred.Id.Equals(orderRequestId) &&
                    pred.OrderRequestState.Equals(OrderRequestState.Pinding)
                );
    }
}
