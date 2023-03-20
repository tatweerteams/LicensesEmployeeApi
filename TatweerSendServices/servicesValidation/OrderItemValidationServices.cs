using Infra;
using Microsoft.EntityFrameworkCore;
using TatweerSendDomain.Domain;
using TatweerSendServices.ExtensionServices;

namespace TatweerSendServices.servicesValidation
{
    public interface IOrderItemValidationServices
    {
        Task<bool> CheckAccountIsExists(string orderIRequestId, string accountId);
        Task<bool> CheckAccountCanNotAddOfDay(string accountId);
        Task<bool> CheckIndividualQuentityOfDay(string accountId, int countChekBook);
        Task<bool> MaxItemInRequestOrder(string orderRequestId, BaseAccountType orderRequestType);

        Task<bool> CheckIsExists(string orderItemId);
        Task<bool> CheckCanChangeState(string orderItemId);

    }

    public class OrderItemValidationServices : IOrderItemValidationServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderItemValidationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<bool> CheckAccountIsExists(string orderIRequestId, string accountId)
            => await _unitOfWork.GetRepositoryReadOnly<OrderItem>().
                   AnyAsync(pred => pred.OrderRequestId.Equals(orderIRequestId) && pred.AccountId.Equals(accountId));


        #region Individual

        public async Task<bool> CheckAccountCanNotAddOfDay(string accountId)
        {

            var result = await _unitOfWork.GetRepositoryReadOnly<OrderItem>().AnyAsync(pred =>
                     pred.AccountId.Equals(accountId) &&
                     (pred.OrderRequest.Branch.BranchSetting.IndividualRequestAccountDay ? false :
                     EF.Functions.DateDiffDay(pred.CreateAt, DateTime.Now) == 0));
            return result;
        }

        public async Task<bool> CheckIndividualQuentityOfDay(string accountId, int countChekBook)
            => await _unitOfWork.GetRepositoryReadOnly<Account>().AnyAsync(pred =>
                pred.Id.Equals(accountId) &&
                pred.OrderItems.Where(w => w.AccountId.Equals(accountId) && EF.Functions.DateDiffDay(w.CreateAt, DateTime.Now) == 0).Sum(a =>
                a.CountChekBook) + countChekBook <= pred.Branch.BranchSetting.IndividualQuentityOfDay);

        public async Task<bool> MaxItemInRequestOrder(string orderRequestId, BaseAccountType orderRequestType)
            => await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().
                                AnyAsync(orderRequestType.MaxItemInRequestOrderExtensionValidation(orderRequestId));

        public async Task<bool> CheckIsExists(string orderItemId)
            => await _unitOfWork.GetRepositoryReadOnly<OrderItem>().AnyAsync(pred => pred.Id.Equals(orderItemId));

        public async Task<bool> CheckCanChangeState(string orderItemId)
            => await _unitOfWork.GetRepositoryReadOnly<OrderItem>().AnyAsync(pred =>
                pred.Id.Equals(orderItemId) &&
                pred.OrderRequest.OrderRequestState.Equals(OrderRequestState.Pinding)
            );



        #endregion
    }
}
