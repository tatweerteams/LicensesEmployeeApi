using CollactionData.DTOs;
using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Queries.PermisstionQueries
{
    public class GetPermisstionQuery : IRequest<ResultOperationDTO<PaginationDto<PermisstionDTO>>>
    {
        public string Name { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
    public class GetPermisstionHandler : IRequestHandler<GetPermisstionQuery, ResultOperationDTO<PaginationDto<PermisstionDTO>>>
    {
        private readonly IPermisstionServices _permisstionServices;
        public GetPermisstionHandler(IPermisstionServices permisstionServices)
        {
            _permisstionServices = permisstionServices;
        }
        public async Task<ResultOperationDTO<PaginationDto<PermisstionDTO>>> Handle(GetPermisstionQuery request, CancellationToken cancellationToken)
        {
            var result = await _permisstionServices.GetPermisstions(request.Name, request.PageNo, request.PageSize);

            return ResultOperationDTO<PaginationDto<PermisstionDTO>>.CreateSuccsessOperation(result);
        }
    }
}
