using CollactionData.Models.PermisstionModel;
using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Commands.PermisstionCommands
{
    public class InsertPermisstionCommand : IRequest<ResultOperationDTO<bool>>
    {
        public InsertPermisstionModel Model { get; set; }
    }

    public class InsertPermisstionHandler : IRequestHandler<InsertPermisstionCommand, ResultOperationDTO<bool>>
    {
        private readonly IPermisstionServices _permisstionServices;
        public InsertPermisstionHandler(IPermisstionServices permisstionServices)
        {
            _permisstionServices = permisstionServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(InsertPermisstionCommand request, CancellationToken cancellationToken)
        {
            await _permisstionServices.InsertPermisstion(request.Model);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "تمت العملية بنجاح" });
        }
    }
}
