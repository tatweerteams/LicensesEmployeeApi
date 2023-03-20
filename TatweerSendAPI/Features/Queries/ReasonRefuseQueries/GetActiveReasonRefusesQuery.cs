using Infra;
using MediatR;
using SharedTatweerSendData.DTOs.ReasonRefuseDTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.ReasonRefuseQueries
{
    public class GetActiveReasonRefusesQuery : IRequest<ResultOperationDTO<IReadOnlyList<ActiveReasonRefuseDTO>>>
    {
        public string Name { get; set; }
    }

    public class GetActiveReasonRefusesHandler : IRequestHandler<GetActiveReasonRefusesQuery,
                    ResultOperationDTO<IReadOnlyList<ActiveReasonRefuseDTO>>>
    {

        private readonly IReasonRefuseServices _reasonRefuseServices;
        public GetActiveReasonRefusesHandler(IReasonRefuseServices reasonRefuseServices)
        {
            _reasonRefuseServices = reasonRefuseServices;
        }
        public async Task<ResultOperationDTO<IReadOnlyList<ActiveReasonRefuseDTO>>> Handle(GetActiveReasonRefusesQuery request, CancellationToken cancellationToken)
        {
            var result = await _reasonRefuseServices.GetActiveReasonRefuses(request.Name);

            return ResultOperationDTO<IReadOnlyList<ActiveReasonRefuseDTO>>.CreateSuccsessOperation(result);

        }
    }
}
