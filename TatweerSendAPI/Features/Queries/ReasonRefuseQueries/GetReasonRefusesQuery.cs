using Infra;
using MediatR;
using SharedTatweerSendData.DTOs.ReasonRefuseDTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.ReasonRefuseQueries
{
    public class GetReasonRefusesQuery : IRequest<ResultOperationDTO<PaginationDto<ReasonRefuseDTO>>>
    {
        public string Name { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }

    public class GetReasonRefusesHandler : IRequestHandler<GetReasonRefusesQuery, ResultOperationDTO<PaginationDto<ReasonRefuseDTO>>>
    {

        private readonly IReasonRefuseServices _reasonRefuseServices;
        public GetReasonRefusesHandler(IReasonRefuseServices reasonRefuseServices)
        {
            _reasonRefuseServices = reasonRefuseServices;
        }
        public async Task<ResultOperationDTO<PaginationDto<ReasonRefuseDTO>>> Handle(GetReasonRefusesQuery request, CancellationToken cancellationToken)
        {
            var result = await _reasonRefuseServices.GetReasonRefuses(request.Name, request.PageNo, request.PageSize);

            return ResultOperationDTO<PaginationDto<ReasonRefuseDTO>>.CreateSuccsessOperation(result);

        }
    }
}
