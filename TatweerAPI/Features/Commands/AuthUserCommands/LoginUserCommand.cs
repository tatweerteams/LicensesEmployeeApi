using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Commands.AuthUserCommands
{
    public class LoginUserCommand : IRequest<ResultOperationDTO<UserAuthDTO>>
    {
        public UserAuthDTO UserAuthSuccess { get; set; }
    }

    public class LoginUserHandler : IRequestHandler<LoginUserCommand, ResultOperationDTO<UserAuthDTO>>
    {
        private readonly IAuthUserServices _authServices;

        public LoginUserHandler(IAuthUserServices authServices)
        {
            _authServices = authServices;
        }
        public async Task<ResultOperationDTO<UserAuthDTO>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var access_Token = await _authServices.SingIn(request.UserAuthSuccess);

            if (string.IsNullOrEmpty(access_Token))
            {
                return ResultOperationDTO<UserAuthDTO>.CreateErrorOperation(messages: new string[] { "لم تتم عملية تسجيل الدخول يجب مراجعة المسؤول عن النظام" });
            }
            request.UserAuthSuccess.AccessToken = access_Token;

            return ResultOperationDTO<UserAuthDTO>.CreateSuccsessOperation(request.UserAuthSuccess);
        }
    }
}
