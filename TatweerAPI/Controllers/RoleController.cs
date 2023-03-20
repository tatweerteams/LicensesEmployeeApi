using CollactionData.DTOs;
using CollactionData.Models.RoleModel;
using IdentityAPI.Features.Commands.RoleCommand;
using IdentityAPI.Features.Queries.RoleQueries;
using IdentityAPI.Filters.RoleFilter;
using Infra;
using Infra.Utili;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : BaseController
    {
        private readonly IMediator _mediator;
        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.RoleManagment)]
        [HttpPost("InsertRole")]
        [TypeFilter(typeof(InsertRoleFilter))]
        public async Task<ResultOperationDTO<bool>> InsertRole([FromBody] InsertRoleModel model, CancellationToken cancellationToken = default)
            => await _mediator.Send(new InsertRoleCommand { Model = model });

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.RoleManagment)]
        [HttpPut("UpdateRole")]
        [TypeFilter(typeof(UpdateRoleFilter))]
        public async Task<ResultOperationDTO<bool>> UpdateRole([FromBody] UpdateRoleModel model, CancellationToken cancellationToken = default)
            => await _mediator.Send(new UpdateRoleCommand { Model = model });

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.RoleManagment)]
        [HttpDelete("DeleteRole")]
        [TypeFilter(typeof(DeleteRoleFilter))]
        public async Task<ResultOperationDTO<bool>> DeleteRole(string roleId, CancellationToken cancellationToken = default)
           => await _mediator.Send(new DeleteRoleCommand { RoleId = roleId });

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.RoleManagment)]
        [HttpPut("ActivationRole")]
        [TypeFilter(typeof(ActivationRoleFilter))]
        public async Task<ResultOperationDTO<bool>> ActivationRole(string roleId, bool isActive, CancellationToken cancellationToken = default)
         => await _mediator.Send(new ActivationRoleCommand { RoleId = roleId, IsActive = isActive });

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.RoleManagment)]
        [HttpGet("GetRoles")]
        public async Task<ResultOperationDTO<PaginationDto<RoleDTO>>> GetRoles(string name, UserTypeState? userType,
                int pageNo = 1, int pageSize = 30, CancellationToken cancellationToken = default)
         => await _mediator.Send(new GetRoleQuery
         {

             Name = name,
             PageNo = pageNo,
             PageSize = pageSize,
             UserType = userType
         });

        [Authorize]
        [HttpGet("GetActiveRoles")]
        public async Task<ResultOperationDTO<IReadOnlyList<ActiveRoleDTO>>> GetActiveRoles(string name, UserTypeState userType, CancellationToken cancellationToken = default)
         => await _mediator.Send(new GetActiveRoleQuery
         {
             UserType = userType,
             RoleName = name,
         });




    }
}
