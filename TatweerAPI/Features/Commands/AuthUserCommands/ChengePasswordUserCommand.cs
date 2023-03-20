using CollactionData.Models.AuthUserModel;
using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Commands.AuthUserCommands
{
    public class ChengePasswordUserCommand : IRequest<ResultOperationDTO<bool>>
    {
        public ChengePasswordModel Model { get; set; }
    }

    public class ChengePasswordUserHandler : IRequestHandler<ChengePasswordUserCommand, ResultOperationDTO<bool>>
    {
        private readonly IAuthUserServices _authServices;

        public ChengePasswordUserHandler(IAuthUserServices authServices)
        {
            _authServices = authServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(ChengePasswordUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _authServices.ChengeUserPassword(request.Model);


            return ResultOperationDTO<bool>.CreateSuccsessOperation(result);
        }
    }
}
