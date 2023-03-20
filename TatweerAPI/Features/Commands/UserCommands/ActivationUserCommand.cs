using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Commands.UserCommands
{
    public class ActivationUserCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string UserId { get; set; }
        public bool IsActive { get; set; }
    }

    public class ActivationUserHandler : IRequestHandler<ActivationUserCommand, ResultOperationDTO<bool>>
    {
        private readonly IUserServices _userServices;
        public ActivationUserHandler(IUserServices userServices)
        {
            _userServices = userServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(ActivationUserCommand request, CancellationToken cancellationToken)
        {
            await _userServices.ActivationUser(request.UserId, request.IsActive);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "تم عملية بنجاح" });
        }
    }



}
