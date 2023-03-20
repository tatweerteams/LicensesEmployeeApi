using CollactionData.DTOs;
using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Queries.PermisstionQueries
{
    public class GetActivePermisstionQuery : IRequest<ResultOperationDTO<IReadOnlyList<ActivePermisstionDTO>>>
    {
        public string Name { get; set; }
    }

    public class GetActivePermisstionHandler : IRequestHandler<GetActivePermisstionQuery, ResultOperationDTO<IReadOnlyList<ActivePermisstionDTO>>>
    {

        private readonly IPermisstionServices _permisstionServices;
        public GetActivePermisstionHandler(IPermisstionServices permisstionServices)
        {
            _permisstionServices = permisstionServices;
        }
        public async Task<ResultOperationDTO<IReadOnlyList<ActivePermisstionDTO>>> Handle(GetActivePermisstionQuery request, CancellationToken cancellationToken)
        {
            var result = await _permisstionServices.GetActivePermisstions(request.Name);

            return ResultOperationDTO<IReadOnlyList<ActivePermisstionDTO>>.CreateSuccsessOperation(result);
        }
    }


}
