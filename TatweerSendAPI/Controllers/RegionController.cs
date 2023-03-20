using Infra;
using Infra.Utili;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedTatweerSendData.DTOs;
using SharedTatweerSendData.Models.RegionModel;
using TatweerSendAPI.Features.Commands.RegionCommands;
using TatweerSendAPI.Features.Queries.RegionQueries;
using TatweerSendAPI.Filters.RegionFilters;

namespace TatweerSendAPI.Controllers
{
    [Route("api/[controller]")]
    public class RegionController : BaseController
    {
        private readonly IMediator _mediator;
        public RegionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = RolesUtili.AddNewRegion + "," + RolesUtili.Administrator)]
        [HttpPost("InsertRegion")]
        [TypeFilter(typeof(InsertRegionFilter))]
        public async Task<ResultOperationDTO<bool>> InsertRegion([FromBody] InsertRegionModel model, CancellationToken cancellationToken = default)
        => await _mediator.Send(new InsertRegionCommand { RegionModel = model, CancellationToken = cancellationToken });

        [Authorize(Roles = RolesUtili.AddNewRegion + "," + RolesUtili.Administrator)]
        [HttpPut("UpdateRegion")]
        [TypeFilter(typeof(UpdateRegionFilter))]
        public async Task<ResultOperationDTO<bool>> UpdateRegion([FromBody] UpdateRegionModel model, CancellationToken cancellationToken = default)
        => await _mediator.Send(new UpdateRegionCommand { RegionModel = model, CancellationToken = cancellationToken });

        [Authorize(Roles = RolesUtili.AddNewRegion + "," + RolesUtili.Administrator)]
        [HttpDelete("DeleteRegion")]
        [TypeFilter(typeof(DeleteRegionFilter))]
        public async Task<ResultOperationDTO<bool>> DeleteRegion(string regionId, CancellationToken cancellationToken = default)
        => await _mediator.Send(new DeleteRegionCommand { RegionId = regionId, CancellationToken = cancellationToken });

        [Authorize(Roles = RolesUtili.AddNewRegion + "," + RolesUtili.Administrator)]
        [HttpGet("GetRegions")]
        public async Task<ResultOperationDTO<IReadOnlyList<RegionDTO>>> GetRegions(string regionName, string regionNo, CancellationToken cancellationToken = default)
        => await _mediator.Send(new GetRegionListQuery { RegionName = regionName, RegionNumber = regionNo, cancellationToken = cancellationToken });

        [Authorize(Roles = RolesUtili.Administrator)]
        [HttpGet("GetRegionUnSelectedBank")]
        public async Task<ResultOperationDTO<IReadOnlyList<RegionDTO>>> GetRegionUnSelectedBank(string bankId, CancellationToken cancellationToken = default)
        => await _mediator.Send(new GetRegionUnSelectBankQuery { BankId = bankId, cancellationToken = cancellationToken });

    }
}
