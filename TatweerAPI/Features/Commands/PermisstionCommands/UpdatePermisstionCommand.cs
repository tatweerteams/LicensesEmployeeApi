using CollactionData.Models.PermisstionModel;
using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Commands.PermisstionCommands
{
    public class UpdatePermisstionCommand : IRequest<ResultOperationDTO<bool>>
    {
        public UpdatePermisstionModel Model { get; set; }
    }

    public class UpdatePermisstionHandler : IRequestHandler<UpdatePermisstionCommand, ResultOperationDTO<bool>>
    {
        private readonly IPermisstionServices _permisstionServices;
        public UpdatePermisstionHandler(IPermisstionServices permisstionServices)
        {
            _permisstionServices = permisstionServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(UpdatePermisstionCommand request, CancellationToken cancellationToken)
        {
            await _permisstionServices.UpdatePermisstion(request.Model);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "تمت العملية بنجاح" });
        }
    }
}
