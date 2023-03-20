using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Commands.RoleCommand
{
    public class DeleteRoleCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string RoleId { get; set; }
    }

    public class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, ResultOperationDTO<bool>>
    {

        private readonly IRoleServices _roleServices;
        public DeleteRoleHandler(IRoleServices roleServices)
        {
            _roleServices = roleServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            await _roleServices.DeleteRole(request.RoleId);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "تم حذف الدور بنجاح" });
        }
    }
}
