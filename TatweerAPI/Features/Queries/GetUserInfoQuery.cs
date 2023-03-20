using FilterAttributeWebAPI.Common;
using IdentityServices.services;
using Infra;
using Infra.Utili;
using MediatR;

namespace IdentityAPI.Features.Queries
{
    public class GetUserInfoQuery : IRequest<ResultOperationDTO<UserAuthDTO>>
    {
    }

    public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, ResultOperationDTO<UserAuthDTO>>
    {
        private readonly IAuthUserServices _authUserServices;
        private readonly HelperUtili _helper;
        public GetUserInfoQueryHandler(IAuthUserServices authUserServices, HelperUtili helper)
        {
            _authUserServices = authUserServices;
            _helper = helper;
        }
        public async Task<ResultOperationDTO<UserAuthDTO>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            var userId = _helper.GetCurrentUser()?.UserID;

            if (string.IsNullOrWhiteSpace(userId))
                throw new ApplicationEx("يجب إعادة تسجيل الدخول");

            var result = await _authUserServices.GetUserInfo(userId);

            var access_Token = await _authUserServices.SingIn(result);

            if (string.IsNullOrEmpty(access_Token))
            {
                return ResultOperationDTO<UserAuthDTO>.CreateErrorOperation(messages: new string[] { "لم تتم عملية تسجيل الدخول يجب مراجعة المسؤول عن النظام" });
            }
            result.AccessToken = access_Token;

            return ResultOperationDTO<UserAuthDTO>.CreateSuccsessOperation(result);

        }
    }
}
