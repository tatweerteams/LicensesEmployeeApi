using CollactionData.Models.RoleModel;
using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Commands.RoleCommand
{
    public class UpdateRoleCommand : IRequest<ResultOperationDTO<bool>>
    {
        public UpdateRoleModel Model { get; set; }
    }

    public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, ResultOperationDTO<bool>>
    {
        private readonly IRoleServices _roleServices;
        public UpdateRoleHandler(IRoleServices roleServices)
        {
            _roleServices = roleServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            await _roleServices.UpdateRole(request.Model);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "تمت تعديل بيانات الدور" });
        }
    }
}
