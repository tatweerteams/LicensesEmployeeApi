using CollactionData.DTOs;
using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Queries.RoleQueries
{
    public class GetRoleQuery : IRequest<ResultOperationDTO<PaginationDto<RoleDTO>>>
    {
        public string Name { get; set; }
        public UserTypeState? UserType { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }

    public class GetRoleHandler : IRequestHandler<GetRoleQuery, ResultOperationDTO<PaginationDto<RoleDTO>>>
    {
        private readonly IRoleServices _roleServices;
        public GetRoleHandler(IRoleServices roleServices)
        {
            _roleServices = roleServices;
        }
        public async Task<ResultOperationDTO<PaginationDto<RoleDTO>>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            var result = await _roleServices.GetRoles(request.Name, request.UserType, request.PageNo, request.PageSize);

            return ResultOperationDTO<PaginationDto<RoleDTO>>.CreateSuccsessOperation(result);
        }
    }
}
