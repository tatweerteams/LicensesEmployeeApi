using CollactionData.Models.RoleModel;
using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Commands.RoleCommand
{
    public class InsertRoleCommand : IRequest<ResultOperationDTO<bool>>
    {
        public InsertRoleModel Model { get; set; }
    }

    public class InsertRoleHandler : IRequestHandler<InsertRoleCommand, ResultOperationDTO<bool>>
    {

        private readonly IRoleServices _roleServices;
        public InsertRoleHandler(IRoleServices roleServices)
        {
            _roleServices = roleServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(InsertRoleCommand request, CancellationToken cancellationToken)
        {
            await _roleServices.InsertRole(request.Model);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "تمت إضافة بيانات الدور" });
        }
    }
}
