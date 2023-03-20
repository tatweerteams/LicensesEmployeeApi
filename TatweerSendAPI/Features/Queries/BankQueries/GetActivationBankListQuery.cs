using Infra;
using MediatR;
using SharedTatweerSendData.DTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.BankQueries
{
    public class GetActivationBankListQuery : IRequest<ResultOperationDTO<IReadOnlyList<ActiveBankDTO>>>
    {
        public string BankName { get; set; }
        public string BankNo { get; set; }
    }

    public class GetActivationBankListHandler : IRequestHandler<GetActivationBankListQuery, ResultOperationDTO<IReadOnlyList<ActiveBankDTO>>>
    {
        private readonly IBankServices _bankServices;
        public GetActivationBankListHandler(IBankServices bankServices)
        {
            _bankServices = bankServices;
        }
        public async Task<ResultOperationDTO<IReadOnlyList<ActiveBankDTO>>> Handle(GetActivationBankListQuery request, CancellationToken cancellationToken)
        {
            var result = await _bankServices.GetActiveBanks(request.BankName, request.BankNo, cancellationToken);
            return ResultOperationDTO<IReadOnlyList<ActiveBankDTO>>.CreateSuccsessOperation(result);
        }
    }

}
