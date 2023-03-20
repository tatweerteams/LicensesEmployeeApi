using Infra;
using MediatR;
using SharedTatweerSendData.DTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.BankQueries
{
    public class GetBankListQuery : IRequest<ResultOperationDTO<IReadOnlyList<BankDTO>>>
    {
        public string BankName { get; set; }
        public string BankNo { get; set; }
        public string RegionId { get; set; }
    }

    public class GetBankListHandler : IRequestHandler<GetBankListQuery, ResultOperationDTO<IReadOnlyList<BankDTO>>>
    {
        private readonly IBankServices _bankServices;
        public GetBankListHandler(IBankServices bankServices)
        {
            _bankServices = bankServices;
        }

        public async Task<ResultOperationDTO<IReadOnlyList<BankDTO>>> Handle(GetBankListQuery request, CancellationToken cancellationToken)
        {
            var result = await _bankServices.
                GetBanks(request.BankName, request.BankNo, request.RegionId, cancellationToken);

            return ResultOperationDTO<IReadOnlyList<BankDTO>>.
                CreateSuccsessOperation(result);
        }
    }
}
