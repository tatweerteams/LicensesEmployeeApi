using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Commands.PermisstionCommands
{
    public class ActivationPermisstionCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
    }

    public class ActivationPermisstionHandler : IRequestHandler<ActivationPermisstionCommand, ResultOperationDTO<bool>>
    {
        private readonly IPermisstionServices _permisstionServices;
        public ActivationPermisstionHandler(IPermisstionServices permisstionServices)
        {
            _permisstionServices = permisstionServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(ActivationPermisstionCommand request, CancellationToken cancellationToken)
        {
            await _permisstionServices.ActivationPermisstion(request.Id, request.IsActive);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "تمت العملية بنجاح" });
        }
    }
}
