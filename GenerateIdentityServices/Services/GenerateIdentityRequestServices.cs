using Events;
using Infra;
using Microsoft.EntityFrameworkCore;
using TatweerSendDomain.Domain;

namespace GenerateIdentityServices.Services
{
    public interface IGenerateIdentityRequestServices
    {
        Task<OrderRequest> GetOrderRequest(string orderRequestId);
        Task UpdateIdentityRequest(OrderRequest orderRequest);

        Task<Branch> GetBranch(string branchId);
        Task UpdateBranch(Branch branch);

        Task<List<OrderItem>> GetOrderItemByOrderId(string orderRequestId);

        Task<List<OrderRequestItemEvent>> GetOrderItemEvntes(string orderRequestId);

    }

    public class GenerateIdentityRequestServices : IGenerateIdentityRequestServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public GenerateIdentityRequestServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public async Task<OrderRequest> GetOrderRequest(string orderRequestId)
        => await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().GetByID(orderRequestId);

        public async Task UpdateIdentityRequest(OrderRequest orderRequest)
            => await _unitOfWork.GetRepositoryWriteOnly<OrderRequest>().Update(orderRequest);


        public async Task<Branch> GetBranch(string branchId)
            => await _unitOfWork.GetRepositoryReadOnly<Branch>().GetByID(branchId);

        public async Task UpdateBranch(Branch branch)
            => await _unitOfWork.GetRepositoryWriteOnly<Branch>().Update(branch);

        public async Task<List<OrderItem>> GetOrderItemByOrderId(string orderRequestId)
            => await _unitOfWork.GetRepositoryReadOnly<OrderItem>().FindBy(
                    predicate: pred => pred.OrderRequestId.Equals(orderRequestId),
                    selector: select => select);


        public async Task<List<OrderRequestItemEvent>> GetOrderItemEvntes(string orderRequestId)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<OrderItem>().FindBy(
                  pred => pred.OrderRequestId.Equals(orderRequestId) && pred.State.Equals(OrderItemState.Success));

            var filterResult = await result.Select(select => new OrderRequestItemEvent
            {
                AccountName = select.AccountName,
                AccountNumber = select.AccountNo,
                BranchName = select.OrderRequest.Branch.Name,
                BranchNumber = select.OrderRequest.Branch.BranchNo,
                ChCount = select.CountChekBook,
                ForCount = select.OrderRequest.ChCount.ToString(),
                FromSerial = select.SerialFrom,
                MyUser = select.OrderRequest.EmployeeNo,
                RegionNumber = select.OrderRequest.Branch.BranchRegion.Region.RegionNo,
                RegName = select.OrderRequest.Branch.BranchRegion.Region.Name,
                RequestDate = select.OrderRequest.CreateAt,
                RequestIdentity = select.OrderRequest.IdentityNumber,
                Tc = select.OrderRequest.OrderRequestType == BaseAccountType.Individual ? "01" :
                    select.OrderRequest.OrderRequestType == BaseAccountType.Companies ? "02" :
                    "03",
                RequestStatus = 1,

            }).ToListAsync();

            return filterResult;
        }
    }
}
