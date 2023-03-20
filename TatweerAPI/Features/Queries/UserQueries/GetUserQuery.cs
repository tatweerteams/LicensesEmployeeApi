using CollactionData.DTOs;
using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Queries.UserQueries
{
    public class GetUserQuery : IRequest<ResultOperationDTO<PaginationDto<UserDTO>>>
    {
        public string BranchId { get; set; }
        public string RegionId { get; set; }
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public UserTypeState? UserType { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }

    }

    public class GetUserHandler : IRequestHandler<GetUserQuery, ResultOperationDTO<PaginationDto<UserDTO>>>
    {
        private readonly IUserServices _userServices;

        public GetUserHandler(IUserServices userServices)
        {
            _userServices = userServices;
        }

        public async Task<ResultOperationDTO<PaginationDto<UserDTO>>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {

            var result = await _userServices.GetUsers(request.BranchId, request.UserType, request.Name,
                request.RegionId, request.EmployeeNo, request.PhoneNumber, request.PageNo, request.PageSize);

            return ResultOperationDTO<PaginationDto<UserDTO>>.CreateSuccsessOperation(result);
        }
    }
}
