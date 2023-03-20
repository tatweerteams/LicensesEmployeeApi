using Infra;
using Infra.Utili;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedTatweerSendData.DTOs;
using SharedTatweerSendData.DTOs.BranchDTOs;
using SharedTatweerSendData.Models.BranchModels;
using TatweerSendAPI.Features.Commands.BranchCommands;
using TatweerSendAPI.Features.Queries.BranchQueries;
using TatweerSendAPI.Filters.BranchFilter;

namespace TatweerSendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    public class BranchController : BaseController
    {

        private readonly IMediator _mediator;
        private readonly HelperUtili _helper;
        public BranchController(IMediator mediator, HelperUtili helper)
        {
            _mediator = mediator;
            _helper = helper;
        }

        [Authorize(Roles = RolesUtili.AddOrUpdateBranch + "," + RolesUtili.Administrator + "," + RolesUtili.AddBranchOtherRegions)]
        [HttpPost("InsertBranch")]
        [TypeFilter(typeof(InsertBranchFilter))]
        public async Task<ResultOperationDTO<bool>> InsertBranch([FromBody] InsertBranchModel model, CancellationToken cancellationToken = default)
            => await _mediator.
                Send(new InsertBranchCommand { BranchModel = model });

        [Authorize(Roles = RolesUtili.AddOrUpdateBranch + "," + RolesUtili.Administrator + "," + RolesUtili.AddBranchOtherRegions)]
        [HttpPut("UpdateBranch")]
        [TypeFilter(typeof(UpdateBranchFilter))]
        public async Task<ResultOperationDTO<bool>> UpdateBranch([FromBody] UpdateBranchModel model, CancellationToken cancellationToken = default)
            => await _mediator.
                Send(new UpdateBranchCommand { BranchModel = model });

        [Authorize(Roles = RolesUtili.AddOrUpdateBranch + "," + RolesUtili.Administrator + "," + RolesUtili.AddBranchOtherRegions)]
        [HttpDelete("DeleteBranch")]
        [TypeFilter(typeof(DeleteBranchFilter))]
        public async Task<ResultOperationDTO<bool>> DeleteBranch(string branchId, CancellationToken cancellationToken = default)
            => await _mediator.
                Send(new DeleteBranchCommand { BranchId = branchId });

        [Authorize(Roles = RolesUtili.AddOrUpdateBranch + "," + RolesUtili.Administrator + "," + RolesUtili.AddBranchOtherRegions)]
        [HttpPut("ActivationBranch")]
        [TypeFilter(typeof(ActivationBranchFilter))]
        public async Task<ResultOperationDTO<bool>> ActivationBranch(string branchId, bool isActive, CancellationToken cancellationToken = default)
            => await _mediator.
                Send(new ActivationBranchCommand { BranchId = branchId, IsActive = isActive });

        [Authorize(Roles = RolesUtili.AddOrUpdateBranch + "," + RolesUtili.Administrator + "," + RolesUtili.AddBranchOtherRegions + "," + RolesUtili.BranchWorkTime + "," + RolesUtili.SettingBranch)]
        [HttpGet("GetBranchs")]
        public async Task<ResultOperationDTO<PaginationDto<BranchDTO>>>
            GetBranchs(string NameOrNumber, string bankId, string branchRegionId, int pageNo = 1, int pageSize = 30)
            => await _mediator.Send(new GetBranchsQuery
            {
                NameOrNumber = NameOrNumber,
                BankId = bankId ?? _helper.GetCurrentUser()?.BankId,
                BranchRegionId = branchRegionId,
                PageNo = pageNo,
                PageSize = pageSize
            });

        [Authorize]
        [HttpGet("GetActiveBranchs")]
        public async Task<ResultOperationDTO<IReadOnlyList<ActiveBranchDTO>>>
            GetActiveBranchs(string NameOrNumber, string branchRegionId)
            => await _mediator.Send(new GetActiveBranchQuery
            {
                BankRegionId = branchRegionId,
                numOrName = NameOrNumber
            });


        [Authorize]
        [HttpPost("InsertBranchList")]
        [TypeFilter(typeof(InsertBranchListFilter))]
        public async Task<ResultOperationDTO<List<ImportBranchList>>> InsertBranchList([FromBody] InsertBranchCollectionModel model, CancellationToken cancellationToken = default)
            => await _mediator.
                Send(new InsertBranchListCommand { BranchModel = model });

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.SettingBranch)]
        [HttpPut("UpdateBranchSetting")]
        [TypeFilter(typeof(UpdateBranchSettingFilter))]
        public async Task<ResultOperationDTO<bool>> UpdateBranchSetting([FromBody] UpdateBranchSettingModel model, CancellationToken cancellationToken = default)
            => await _mediator.
                Send(new UpdateBranchSettingCommand { BranchSetting = model });

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.SettingBranch)]
        [HttpGet("GetBranchSetting")]
        public async Task<ResultOperationDTO<BranchSettingDTO>>
            GetBranchSetting(string branchSettingId)
            => await _mediator.Send(new GetBranchSettingQuery
            {
                BranchSetiingId = branchSettingId,
            });

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.BranchWorkTime)]
        [HttpGet("GetBranchWorkTime")]
        public async Task<ResultOperationDTO<IReadOnlyList<BranchWorkTimeDTO>>>
            GetBranchWorkTime(string branchId)
            => await _mediator.Send(new BranchWorkTimeQuery
            {
                BranchId = branchId,
            });

        [Authorize]
        [HttpPut("UpdateBranchWorkTime")]
        [TypeFilter(typeof(UpdateBranchWorkTimeFilter))]
        public async Task<ResultOperationDTO<bool>> UpdateBranchWorkTime([FromBody] BranchWorkTimeModel model, CancellationToken cancellationToken = default)
           => await _mediator.
               Send(new UpdateBranchWorkTimeCommand { BranchWorkTime = model });

        [Authorize]
        [HttpPut("ActivationBranchWorkTime")]
        [TypeFilter(typeof(ActivationBranchWorkTimeFilter))]
        public async Task<ResultOperationDTO<bool>> ActivationBranchWorkTime(string branchWorkTimeId, bool isActive, CancellationToken cancellationToken = default)
          => await _mediator.
              Send(new ActivationBranchWorkTimeCommand { BranchWorkTimeId = branchWorkTimeId, IsActive = isActive });

        [Authorize]
        [HttpPut("UpdateAllWorkTime")]
        //[TypeFilter(typeof(UpdateBranchWorkTimeFilter))]
        public async Task<ResultOperationDTO<bool>> UpdateAllWorkTime([FromBody] UpdateAllWorkTime updateAllWork, CancellationToken cancellationToken = default)
          => await _mediator.
              Send(new UpdateAllWorkTimeCommand
              {
                  TimeEnd = updateAllWork.TimeEnd,
                  TimeStrart = updateAllWork.TimeStart,
                  Days = updateAllWork.Days,
                  IsActive = updateAllWork.IsActive,
              });

    }

    public class UpdateAllWorkTime
    {
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public List<DayOfWeek> Days { get; set; }
        public bool IsActive { get; set; }
    }
}
