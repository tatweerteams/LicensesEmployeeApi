using CollactionData.DTOs;
using CollactionData.Models.Users;
using IdentityAPI.Features.Commands.UserCommands;
using IdentityAPI.Features.Queries.UserQueries;
using IdentityAPI.Filters.UserFilters;
using Infra;
using Infra.Utili;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.UserManagment + "," + RolesUtili.AddUserOtherBranchs)]
        [HttpPost("InsertUser")]
        [TypeFilter(typeof(InsertUserFilter))]
        public async Task<ResultOperationDTO<bool>> InsertUser([FromBody] InsertUserModel model, CancellationToken cancellationToken = default)
            => await _mediator.Send(new InsertUserCommand { Model = model });

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.UserManagment + "," + RolesUtili.AddUserOtherBranchs)]
        [HttpPut("UpdateUser")]
        [TypeFilter(typeof(UpdateUserFilter))]
        public async Task<ResultOperationDTO<bool>> UpdateUser([FromBody] UpdateUserModel model, CancellationToken cancellationToken = default)
            => await _mediator.Send(new UpdateUserCommand { Model = model });

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.UserManagment + "," + RolesUtili.AddUserOtherBranchs)]
        [HttpPut("ActivationUser")]
        [TypeFilter(typeof(CheckUserFilter))]
        public async Task<ResultOperationDTO<bool>> ActivationUser(string userId, bool isActive, CancellationToken cancellationToken = default)
            => await _mediator.Send(new ActivationUserCommand { UserId = userId, IsActive = isActive });

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.UserManagment + "," + RolesUtili.AddUserOtherBranchs)]
        [HttpPut("ResetPassword")]
        [TypeFilter(typeof(CheckUserFilter))]
        public async Task<ResultOperationDTO<bool>> ResetPassword(string userId, string newPassword, bool sendPassword = false, CancellationToken cancellationToken = default)
           => await _mediator.Send(new ResetPasswordCommand
           {
               UserId = userId,
               NewPassword = newPassword,
               SendPassword = sendPassword
           });

        [Authorize(Roles = RolesUtili.Administrator + "," + RolesUtili.UserManagment + "," + RolesUtili.AddUserOtherBranchs)]
        [HttpGet("GetUsers")]
        public async Task<ResultOperationDTO<PaginationDto<UserDTO>>> GetUsers(string name, UserTypeState? userType,
               string branchId, string regionId, string employeeNo, string phoneNumber, int pageNo = 1, int pageSize = 30, CancellationToken cancellationToken = default)
         => await _mediator.Send(new GetUserQuery
         {

             Name = name,
             UserType = userType,
             BranchId = branchId,
             RegionId = regionId,
             EmployeeNo = employeeNo,
             PhoneNumber = phoneNumber,
             PageNo = pageNo,
             PageSize = pageSize,
         });



    }
}
