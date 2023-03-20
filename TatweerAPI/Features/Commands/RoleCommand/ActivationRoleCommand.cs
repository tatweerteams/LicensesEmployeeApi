using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Commands.RoleCommand
{
    public class ActivationRoleCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string RoleId { get; set; }
        public bool IsActive { get; set; }
    }

    public class ActivationRoleHandler : IRequestHandler<ActivationRoleCommand, ResultOperationDTO<bool>>
    {
        private readonly IRoleServices _roleServices;
        public ActivationRoleHandler(IRoleServices roleServices)
        {
            _roleServices = roleServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(ActivationRoleCommand request, CancellationToken cancellationToken)
        {
            await _roleServices.ActivationRole(request.RoleId, request.IsActive);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "تمت عملية بنجاح" });
        }
    }
}
