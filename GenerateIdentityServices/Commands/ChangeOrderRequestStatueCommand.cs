using GenerateIdentityServices.Services;
using Infra;
using MediatR;

namespace GenerateIdentityServices.Commands
{
    public class ChangeOrderRequestStatueCommand : IRequest<ResultOperationDTO<bool>>
    {
        public List<string> IdentityNumbers { get; set; }
        public OrderRequestState OrderRequestState { get; set; }
    }
    public class ChangeOrderRequestStatueHandler : IRequestHandler<ChangeOrderRequestStatueCommand, ResultOperationDTO<bool>>
    {
        private readonly IChangeOrderRequestStatueServices _services;
        public ChangeOrderRequestStatueHandler(IChangeOrderRequestStatueServices services)
        {
            _services = services;
        }
        public async Task<ResultOperationDTO<bool>> Handle(ChangeOrderRequestStatueCommand request, CancellationToken cancellationToken)
        {
            await _services.ChngeOrderRequestStatu(request.IdentityNumbers, request.OrderRequestState);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true);
        }
    }

}
