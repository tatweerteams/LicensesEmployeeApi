using Infra;
using Infra.Utili;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedTatweerSendData.DTOs;
using SharedTatweerSendData.Models;
using TatweerSendAPI.Features.Commands.BankRegionCommands;
using TatweerSendAPI.Features.Queries.BankRegionQueries;
using TatweerSendAPI.Filters.BankRegionFilter;

namespace TatweerSendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BankRegionController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly HelperUtili _helper;
        public BankRegionController(IMediator mediator, HelperUtili helper)
        {
            _mediator = mediator;
            _helper = helper;
        }
        [Authorize(Roles = RolesUtili.RegionManagment + "," + RolesUtili.Administrator)]
        [HttpPost("InsertBankRegion")]
        [TypeFilter(typeof(InsertBankRegionFilter))]
        public async Task<ResultOperationDTO<bool>> InsertBank([FromBody] InsertBankRegionModel model, CancellationToken cancellationToken = default)
            => await _mediator.Send(new InsertBankRegionCommand { model = model });

        [Authorize(Roles = RolesUtili.RegionManagment + "," + RolesUtili.Administrator)]
        [HttpPut("ActivationBankRegion")]
        [TypeFilter(typeof(ActivationBankRegionFilter))]
        public async Task<ResultOperationDTO<bool>> ActivationBankRegion(string bankRegionId, bool isActive, CancellationToken cancellationToken = default)
            => await _mediator.Send(new ActivationBankRegionCommand { BankRegionId = bankRegionId, IsActive = isActive });


        [Authorize(Roles = RolesUtili.RegionManagment + "," + RolesUtili.Administrator)]
        [HttpDelete("DeleteBankRegion")]
        [TypeFilter(typeof(DeleteBankRegionFilter))]
        public async Task<ResultOperationDTO<bool>> DeleteBankRegion(string bankRegionId, CancellationToken cancellationToken = default)
            => await _mediator.Send(new DeleteBankRegionCommand { BankRegionId = bankRegionId });


        [Authorize(Roles = RolesUtili.RegionManagment + "," + RolesUtili.Administrator)]
        [HttpGet("GetBankRegions")]
        public async Task<ResultOperationDTO<PaginationDto<BankRegionDTO>>> GetBankRegions(
            string bankId, string regionName, string regionNo,
            int pageNo = 1, int pageSize = 30,
            CancellationToken cancellationToken = default)
            => await _mediator.Send(new BankRegionAllQuery
            {

                BankId = bankId ?? _helper.GetCurrentUser()?.BankId,
                RegionName = regionName,
                RegionNo = regionNo,
                PageNo = pageNo,
                PageSize = pageSize,

            });

        [Authorize]
        [HttpGet("GetActiveBankRegions")]
        public async Task<ResultOperationDTO<IReadOnlyList<BankRegionActiveDTO>>> GetActiveBankRegions(
            string bankId, string regionName, string regionNo,
            CancellationToken cancellationToken = default)
            => await _mediator.Send(new ListBankRegionActiveQuery
            {
                BankId = bankId ?? _helper.GetCurrentUser()?.BankId,
                RegionName = regionName,
                RegionNo = regionNo
            });
    }
}
