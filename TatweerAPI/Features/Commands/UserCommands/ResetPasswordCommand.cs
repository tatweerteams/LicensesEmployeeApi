using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Commands.UserCommands
{
    public class ResetPasswordCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string UserId { get; set; }
        public string NewPassword { get; set; }
        public bool SendPassword { get; set; }

    }

    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, ResultOperationDTO<bool>>
    {
        private readonly IUserServices _userServices;
        public ResetPasswordHandler(IUserServices userServices)
        {
            _userServices = userServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            await _userServices.ResetPassword(request.UserId, request.NewPassword, request.SendPassword);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "تم إعادة ضبط كلمة المرور بنجاح" });
        }
    }
}
