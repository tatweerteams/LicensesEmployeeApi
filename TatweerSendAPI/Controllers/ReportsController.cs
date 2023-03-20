using Infra;
using Infra.Utili;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedTatweerSendData.DTOs.ReportDTOs;
using TatweerSendAPI.Features.Queries.ReportQueries;

namespace TatweerSendAPI.Controllers
{
    [Route("api/[controller]")]
    public class ReportsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly HelperUtili _helper;
        public ReportsController(IMediator mediator, HelperUtili helper)
        {
            _helper = helper;
            _mediator = mediator;
        }

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.BranchAllReport + "," + RolesUtili.accountRequestReport)]
        [HttpGet("GetAccountRequestReport")]
        public async Task<ResultOperationDTO<PaginationDto<AccountRequestReportDTO>>>
           GetAccountRequestReport(string branchId, string accountNo, BaseAccountType? accountType,
            string phoneNo, DateTime? from, DateTime? to, int pageNo = 1, int pageSize = 30)
           => await _mediator.Send(new GetAccountRequestQuery
           {
               BranchId = branchId ?? _helper.GetCurrentUser().BranchId,
               AccountNo = accountNo,
               AccountType = accountType,
               PhoneNo = phoneNo,
               FromDate = from,
               ToDate = to,
               PageNo = pageNo,
               PageSize = pageSize
           });

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.BranchAllReport + "," + RolesUtili.empolyeeReport)]
        [HttpGet("GetEmpolyeeReport")]
        public async Task<ResultOperationDTO<PaginationDto<EmployeeReportDTO>>>
           GetEmpolyeeReport(string branchId, string employeeNo, DateTime? from, DateTime? to, int pageNo = 1, int pageSize = 30)
           => await _mediator.Send(new GetEmpolyeeReportQuery
           {
               BranchId = branchId ?? _helper.GetCurrentUser().BranchId,
               EmployeeNo = employeeNo,
               FromDate = from,
               ToDate = to,
               PageNo = pageNo,
               PageSize = pageSize
           });

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.BranchAllReport + "," + RolesUtili.branchOrderReport)]
        [HttpGet("GetBranchReport")]
        public async Task<ResultOperationDTO<PaginationDto<BranchOrderReportDTO>>>
           GetBranchReport(string branchId, string identityNo, OrderRequestState? orderRequestState,
            BaseAccountType? orderRequestType, InputTypeState? inputType, DateTime? from, DateTime? to, int pageNo = 1, int pageSize = 30)
           => await _mediator.Send(new GetBranchReportQuery
           {
               BranchId = branchId ?? _helper.GetCurrentUser().BranchId,
               IdentityNo = identityNo,
               OrderRequestState = orderRequestState,
               OrderRequestType = orderRequestType,
               InputType = inputType,
               FromDate = from,
               ToDate = to,
               PageNo = pageNo,
               PageSize = pageSize
           });

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.BranchAllReport + "," + RolesUtili.statisticBranchReport)]
        [HttpGet("GetStatisticBranchReport")]
        public async Task<ResultOperationDTO<PaginationDto<StatisticBranchDTO>>>
           GetStatisticBranchReport(string nameOrNumber, string bankId, int pageNo = 1, int pageSize = 30)
           => await _mediator.Send(new GetStatisticBranchsQuery
           {
               NameOrNumber = nameOrNumber,
               BankId = bankId ?? _helper.GetCurrentUser().BankId,
               PageNo = pageNo,
               PageSize = pageSize
           });

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.BranchAllReport + "," + RolesUtili.orderRequestPriteOut)]
        [HttpGet("GetOrderRequestPriteOutReport")]
        public async Task<ResultOperationDTO<PaginationDto<OrderRequestPriteOutDTO>>>
         GetOrderRequestPriteOutReport(string branchId, string identityNo, string fromSerial, string toSerial,
          BaseAccountType? orderRequestType, DateTime? from, DateTime? to, int pageNo = 1, int pageSize = 30)
         => await _mediator.Send(new GetOrderRequestPriteOutReportQuery
         {
             BranchId = branchId ?? _helper.GetCurrentUser().BranchId,
             IdentityNo = identityNo,
             OrderRequestType = orderRequestType,
             ToSerial = toSerial,
             FromSerial = fromSerial,
             FromDate = from,
             ToDate = to,
             PageNo = pageNo,
             PageSize = pageSize
         });



    }
}
