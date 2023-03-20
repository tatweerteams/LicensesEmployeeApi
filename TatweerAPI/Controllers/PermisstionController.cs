using CollactionData.DTOs;
using CollactionData.Models.PermisstionModel;
using IdentityAPI.Features.Commands.PermisstionCommands;
using IdentityAPI.Features.Queries.PermisstionQueries;
using IdentityAPI.Filters.PermisstionFilters;
using Infra;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers
{
    [Route("api/[controller]")]

    public class PermisstionController : BaseController
    {
        private readonly IMediator _mediator;
        public PermisstionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "AdminSystem")]
        [HttpGet("GetPermisstions")]
        public async Task<ResultOperationDTO<PaginationDto<PermisstionDTO>>>
            GetPermisstions(string name, int pageNo = 1, int pageSize = 30, CancellationToken cancellationToken = default)
                => await _mediator.Send(new GetPermisstionQuery
                {
                    Name = name,
                    PageNo = pageNo,
                    PageSize = pageSize,
                });


        [HttpGet("GetActivePermisstion")]
        public async Task<ResultOperationDTO<IReadOnlyList<ActivePermisstionDTO>>>
            GetActivePermisstion(string name, CancellationToken cancellationToken = default)
                => await _mediator.Send(new GetActivePermisstionQuery { Name = name });

        [Authorize(Roles = "AdminSystem")]
        [HttpPut("ActivationPermisstion")]
        [TypeFilter(typeof(ActivationPermisstionFilter))]
        public async Task<ResultOperationDTO<bool>> ActivationPermisstion(string id, bool isActive, CancellationToken cancellationToken = default)
            => await _mediator.Send(new ActivationPermisstionCommand { Id = id, IsActive = isActive });


        [Authorize(Roles = "AdminSystem")]
        [HttpPost("InsertPermisstion")]
        [TypeFilter(typeof(InsertPermisstionFilter))]
        public async Task<ResultOperationDTO<bool>> InsertPermisstion([FromBody] InsertPermisstionModel model, CancellationToken cancellationToken = default)
            => await _mediator.Send(new InsertPermisstionCommand { Model = model });


        [Authorize(Roles = "AdminSystem")]
        [HttpPut("UpdatePermisstion")]
        [TypeFilter(typeof(UpdatePermisstionFilter))]
        public async Task<ResultOperationDTO<bool>> UpdatePermisstion([FromBody] UpdatePermisstionModel model, CancellationToken cancellationToken = default)
            => await _mediator.Send(new UpdatePermisstionCommand { Model = model });
    }
}
