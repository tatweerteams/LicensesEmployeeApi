using CollactionData.DTOs;
using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Queries.RoleQueries
{
    public class GetActiveRoleQuery : IRequest<ResultOperationDTO<IReadOnlyList<ActiveRoleDTO>>>
    {
        public UserTypeState UserType { get; set; }
        public string RoleName { get; set; }
    }

    public class GetActiveRoleHandler : IRequestHandler<GetActiveRoleQuery, ResultOperationDTO<IReadOnlyList<ActiveRoleDTO>>>
    {
        private readonly IRoleServices _roleServices;
        public GetActiveRoleHandler(IRoleServices roleServices)
        {
            _roleServices = roleServices;
        }

        public async Task<ResultOperationDTO<IReadOnlyList<ActiveRoleDTO>>> Handle(GetActiveRoleQuery request, CancellationToken cancellationToken)
        {
            var result = await _roleServices.GetActiveRoles(request.UserType, request.RoleName);

            return ResultOperationDTO<IReadOnlyList<ActiveRoleDTO>>.CreateSuccsessOperation(result);
        }
    }
}
