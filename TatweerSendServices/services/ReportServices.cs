using Infra;
using Microsoft.EntityFrameworkCore;
using SharedTatweerSendData.DTOs.ReportDTOs;
using TatweerSendDomain.Domain;
using TatweerSendServices.ExtensionServices;

namespace TatweerSendServices.services
{
    public interface IReportServices
    {
        Task<PaginationDto<AccountRequestReportDTO>> GetAccountRequest(string branchId, string accountNo, BaseAccountType? accountType,
            string phoneNo, DateTime? from, DateTime? to, int pageNo = 1, int pageSize = 30);
        Task<PaginationDto<EmployeeReportDTO>> GetEmpolyeeReport(string branchId, string employeeNo,
           DateTime? from, DateTime? to, int pageNo = 1, int pageSize = 30);

        Task<PaginationDto<BranchOrderReportDTO>> GetBranchReport(string branchId, string identityNo, OrderRequestState? orderRequestState,
            BaseAccountType? orderRequestType, InputTypeState? inputType, DateTime? from, DateTime? to, int pageNo = 1, int pageSize = 30);

        Task<PaginationDto<StatisticBranchDTO>> GetStatisticBranchs(string nameOrNumber, string bankId, int pageNo = 1, int pageSize = 30);
        Task<PaginationDto<OrderRequestPriteOutDTO>> GetOrderRequestPriteOutReport(string branchId, string identityNo,
        BaseAccountType? orderRequestType, string fromSerial, string toSerial, DateTime? from, DateTime? to, int pageNo = 1, int pageSize = 30);

    }

    public class ReportServices : IReportServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReportServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PaginationDto<AccountRequestReportDTO>> GetAccountRequest(string branchId, string accountNo, BaseAccountType? accountType, string phoneNo, DateTime? from, DateTime? to, int pageNo = 1, int pageSize = 30)
        {




            var result = (await _unitOfWork.GetRepositoryReadOnly<OrderItem>().FindBy(
                predicate: accountNo.SearchAccountRequestExpression(branchId, accountType, phoneNo, from, to)
                ,
                selector: select => new AccountRequestReportDTO
                {
                    AccountName = select.AccountName,
                    AccountNo = select.AccountNo,
                    BranchName = select.OrderRequest.Branch.Name,
                    IdentityNumber = select.OrderRequest.IdentityNumber,
                    RequestAt = select.OrderRequest.CreateAt,
                    RequestDate = select.OrderRequest.CreateAt.ToString("yyyy-MM-dd"),
                    OrderItemState = select.State,
                    OrderRequestState = select.OrderRequest.OrderRequestState,
                    OrderRequestNote = select.OrderRequest.OrderEvents.OrderByDescending(o => o.OrderCreationDate).
                    FirstOrDefault().RejectNote,
                },
                pageNo: pageNo,
                pageSize: pageSize)).OrderByDescending(order => order.RequestAt).ToList();

            var totalRecordCount = await _unitOfWork.GetRepositoryReadOnly<OrderItem>().
                GetCount(accountNo.SearchAccountRequestExpression(branchId, accountType, phoneNo, from, to));

            return new PaginationDto<AccountRequestReportDTO>()
            {
                Data = result,
                PageCount = totalRecordCount > 0
            ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
            : 0
            };


        }


        public async Task<PaginationDto<EmployeeReportDTO>> GetEmpolyeeReport(string branchId, string employeeNo, DateTime? from, DateTime? to, int pageNo = 1, int pageSize = 30)
        {
            var result = (await _unitOfWork.GetRepositoryReadOnly<OrderEvent>().FindBy(
                predicate: branchId.SearchEmpolyeeReportExpression(employeeNo, from, to),
                selector: select => new EmployeeReportDTO
                {
                    BranchName = select.OrderRequest.Branch.Name,
                    IdentityNumber = select.OrderRequest.IdentityNumber,
                    OrderRequestNote = select.RejectNote,
                    OrderRequestState = select.OrderRequestState,
                    RequestAt = select.OrderRequest.CreateAt,
                    RequestDate = select.OrderRequest.CreateAt.ToString("yyyy-MM-dd"),
                    OrderCreationDate = select.OrderCreationDate.ToString("yyyy-MM-dd"),
                    EmployeeNo = select.EmployeeNo
                },
                pageNo: pageNo,
                pageSize: pageSize)).OrderBy(o => o.IdentityNumber).ToList();

            var totalRecordCount = await _unitOfWork.GetRepositoryReadOnly<OrderEvent>().
                 GetCount(branchId.SearchEmpolyeeReportExpression(employeeNo, from, to));

            return new PaginationDto<EmployeeReportDTO>()
            {
                Data = result,
                PageCount = totalRecordCount > 0
            ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
            : 0
            };
        }

        public async Task<PaginationDto<BranchOrderReportDTO>> GetBranchReport(string branchId, string identityNo,
            OrderRequestState? orderRequestState, BaseAccountType? orderRequestType, InputTypeState? inputType,
            DateTime? from, DateTime? to, int pageNo = 1, int pageSize = 30)
        {

            var filterData = await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().
                FindBy(branchId.SearchBranchOrderReportExpression(identityNo, orderRequestState, orderRequestType, inputType, from, to));

            var totalRecordCount = await filterData.CountAsync();
            var result = await filterData.Select(select => new BranchOrderReportDTO
            {
                BranchName = select.Branch.Name,
                BranchNumber = select.Branch.BranchNo,
                IdentityNumber = select.IdentityNumber,
                OrderRequestId = select.Id,
                OrderRequestState = select.OrderRequestState,
                OrderRequestType = select.OrderRequestType,
                RequestAt = select.CreateAt,
                RequestDate = select.CreateAt.ToString("yyyy-MM-dd"),
                CountChekBook = select.OrderItems.Where(w => w.State.Equals(OrderItemState.Success)).Sum(s => s.CountChekBook),
                InputType = select.InputTypeState,
                EmployeeNo = select.EmployeeNo,
                OrderRequestNote = select.OrderEvents.OrderByDescending(o => o.OrderCreationDate).
                    FirstOrDefault(s => !string.IsNullOrEmpty(s.RejectNote)).RejectNote ?? null,
            }).
            Skip((pageNo - 1) * pageSize).
            Take(pageSize).
            OrderBy(o => o.RequestAt).
            ToListAsync();

            return new PaginationDto<BranchOrderReportDTO>()
            {
                Data = result,
                PageCount = totalRecordCount > 0
            ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
            : 0
            };

        }

        public async Task<PaginationDto<StatisticBranchDTO>> GetStatisticBranchs(string nameOrNumber, string bankId, int pageNo = 1, int pageSize = 30)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<Branch>().FindBy(
                predicate: pred => pred.BranchRegion.BankId.Equals(bankId) &&
                (string.IsNullOrWhiteSpace(nameOrNumber) || (pred.Name.Contains(nameOrNumber) || pred.BranchNo.Contains(nameOrNumber))),
                selector: select => new StatisticBranchDTO
                {
                    AccountCount = select.Accounts.Count(),
                    BranchName = select.Name,
                    BranchNumber = select.BranchNo,
                    RegionName = select.BranchRegion.Region.Name,
                    OrderRequestCount = select.OrderRequests.Count(),
                    OrderRequestIndividualCount = select.OrderRequests.Count(c => c.OrderRequestType.Equals(BaseAccountType.Individual)),
                    OrderRequestCompanyCount = select.OrderRequests.Count(c => c.OrderRequestType.Equals(BaseAccountType.Companies)),

                },
                pageNo: pageNo,
                pageSize: pageSize);

            var totalRecordCount = await _unitOfWork.GetRepositoryReadOnly<Branch>().
            GetCount(pred => pred.BranchRegion.BankId.Equals(bankId) && (string.IsNullOrWhiteSpace(nameOrNumber) || (pred.Name.Contains(nameOrNumber) || pred.BranchNo.Contains(nameOrNumber))));

            return new PaginationDto<StatisticBranchDTO>()
            {
                Data = result.OrderBy(o => o.BranchNumber).ToList(),
                PageCount = totalRecordCount > 0
            ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
            : 0
            };
        }

        public async Task<PaginationDto<OrderRequestPriteOutDTO>> GetOrderRequestPriteOutReport(string branchId, string identityNo,
        BaseAccountType? orderRequestType, string fromSerial, string toSerial, DateTime? from, DateTime? to, int pageNo = 1, int pageSize = 30)
        {
            var result = (await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().FindBy(
                 predicate: branchId.SearchBranchOrderReportExpression(identityNo, null, orderRequestType, null, from, to, printOutCenter: true),
                 selector: select => new OrderRequestPriteOutDTO
                 {
                     BranchName = select.Branch.Name,
                     BranchNumber = select.Branch.BranchNo,
                     IdentityNumber = select.IdentityNumber,
                     OrderRequestId = select.Id,
                     OrderRequestState = select.OrderRequestState,
                     OrderRequestType = select.OrderRequestType,
                     RequestAt = select.CreateAt,
                     RequestDate = select.CreateAt.ToString("yyyy-MM-dd"),
                     InputType = select.InputTypeState,
                     EmployeeNo = select.EmployeeNo,
                     OrderRequestNote = select.OrderEvents.OrderByDescending(o => o.OrderCreationDate).
                     FirstOrDefault(s => s.OrderRequestState.Equals(OrderRequestState.RejectRequest)).RejectNote,
                     ToSerial = select.OrderItems.OrderByDescending(o => o.SerialFrom).FirstOrDefault().SerialFrom,
                     FromSerial = select.OrderItems.OrderBy(o => o.SerialFrom).FirstOrDefault().SerialFrom,
                 },
                 pageNo: pageNo,
                 pageSize: pageSize)).OrderBy(o => o.RequestAt).ToList();

            var totalRecordCount = await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().
                 GetCount(branchId.SearchBranchOrderReportExpression(identityNo, null, orderRequestType, null, from, to, printOutCenter: true));

            return new PaginationDto<OrderRequestPriteOutDTO>()
            {
                Data = result,
                PageCount = totalRecordCount > 0
            ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
            : 0
            };
        }
    }
}
