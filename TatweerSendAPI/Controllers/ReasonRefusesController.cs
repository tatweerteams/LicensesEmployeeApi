using Infra;
using Infra.Utili;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedTatweerSendData.DTOs.ReasonRefuseDTOs;
using SharedTatweerSendData.Models.ReasonRefuseModel;
using TatweerSendAPI.Features.Commands.ReasonRefuseCommands;
using TatweerSendAPI.Features.Queries.ReasonRefuseQueries;
using TatweerSendAPI.Filters.ReasonRefuseFilter;

namespace TatweerSendAPI.Controllers
{
    [Route("api/[controller]")]
    public class ReasonRefusesController : BaseController
    {
        private readonly IMediator _mediator;
        public ReasonRefusesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = RolesUtili.ReasonRefuseManagment + "," + RolesUtili.Administrator)]
        //[Authorize(Roles = "Test")]
        [HttpPost("InsertReasonRefuse")]
        [TypeFilter(typeof(InsertReasonRefuseFilter))]
        public async Task<ResultOperationDTO<bool>> InsertReasonRefuse([FromBody] InsertReasonRefuseModel model, CancellationToken cancellationToken = default)
        => await _mediator.Send(new InsertReasonRefuseCommand { Model = model });

        [Authorize(Roles = RolesUtili.ReasonRefuseManagment + "," + RolesUtili.Administrator)]
        [HttpPut("UpdateReasonRefuse")]
        [TypeFilter(typeof(UpdateReasonRefuseFilter))]
        public async Task<ResultOperationDTO<bool>> UpdateReasonRefuse([FromBody] UpdateReasonRefuseModel model, CancellationToken cancellationToken = default)
        => await _mediator.Send(new UpdateReasonRefuseCommand { Model = model });

        [Authorize(Roles = RolesUtili.ReasonRefuseManagment + "," + RolesUtili.Administrator)]
        [HttpPut("ActivationReasonRefuse")]
        [TypeFilter(typeof(ReasonRefuseFilter))]
        public async Task<ResultOperationDTO<bool>> ActivationReasonRefuse(int reasonRefuseId, bool isActive, CancellationToken cancellationToken = default)
        => await _mediator.Send(new ActivationReasonRefuseCommand { ReasonRefuseId = reasonRefuseId, IsActive = isActive });

        [Authorize(Roles = RolesUtili.ReasonRefuseManagment + "," + RolesUtili.Administrator)]
        [HttpGet("GetReasonRefuses")]
        public async Task<ResultOperationDTO<PaginationDto<ReasonRefuseDTO>>>
            GetReasonRefuses(string name, int pageNo = 1, int pageSize = 30, CancellationToken cancellationToken = default)
          => await _mediator.Send(new GetReasonRefusesQuery { Name = name, PageNo = pageNo, PageSize = pageSize });

        [Authorize]
        [HttpGet("GetActiveReasonRefuses")]
        public async Task<ResultOperationDTO<IReadOnlyList<ActiveReasonRefuseDTO>>> GetActiveReasonRefuses(string name, CancellationToken cancellationToken = default)
          => await _mediator.Send(new GetActiveReasonRefusesQuery { Name = name });

        [Authorize(Roles = RolesUtili.ReasonRefuseManagment + "," + RolesUtili.Administrator)]
        [HttpDelete("DeleteReasonRefuse")]
        [TypeFilter(typeof(ReasonRefuseFilter))]
        public async Task<ResultOperationDTO<bool>> DeleteReasonRefuse(int reasonRefuseId, CancellationToken cancellationToken = default)
        => await _mediator.Send(new DeleteReasonRefuseCommand { ReasonRefuseId = reasonRefuseId });
    }
}
