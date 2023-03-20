using Infra;
using MediatR;
using SharedTatweerSendData.DTOs;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Queries.BankRegionQueries
{
    public class ListBankRegionActiveQuery : IRequest<ResultOperationDTO<IReadOnlyList<BankRegionActiveDTO>>>
    {
        public string BankId { get; set; }
        public string RegionName { get; set; }
        public string RegionNo { get; set; }
    }

    public class ListBankRegionActiveHandler : IRequestHandler<ListBankRegionActiveQuery, ResultOperationDTO<IReadOnlyList<BankRegionActiveDTO>>>
    {
        private readonly IBankRegionServices _bankRegionServices;

        public ListBankRegionActiveHandler(IBankRegionServices bankRegionServices)
        {
            _bankRegionServices = bankRegionServices;
        }
        public async Task<ResultOperationDTO<IReadOnlyList<BankRegionActiveDTO>>> Handle(ListBankRegionActiveQuery request, CancellationToken cancellationToken)
        {
            var result = await _bankRegionServices.GetActive(request.BankId, request.RegionName, request.RegionNo, cancellationToken);

            return ResultOperationDTO<IReadOnlyList<BankRegionActiveDTO>>.CreateSuccsessOperation(result);
        }
    }

}
