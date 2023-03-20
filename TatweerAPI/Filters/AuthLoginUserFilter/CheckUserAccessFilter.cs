using CollactionData.Models.AuthUserModel;
using IdentityServices.services;
using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityAPI.Filters.AuthLoginUserFilter
{
    public class CheckUserAccessFilter : ActionFilterAttribute
    {
        private readonly IAuthUserServices _authUserServices;

        private readonly HelperUtili _helper;
        public CheckUserAccessFilter(IAuthUserServices authUserServices, HelperUtili helper)
        {
            _authUserServices = authUserServices;
            _helper = helper;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var param = context.ActionArguments.TryGetValue("loginUser", out var _loginUser);
            if (_loginUser is LoginUserModel loginUser)
            {

                var isAdminSystem = await _authUserServices.CheckIsAdminSystem(loginUser.NameOrNumber, loginUser.Password);
                switch (isAdminSystem.CheckIsAdminState)
                {
                    case CheckIsAdminState.IsAdmin:
                        context.Result = new OkObjectResult(ResultOperationDTO<UserAuthDTO>.
                            CreateSuccsessOperation(isAdminSystem));
                        return;

                    case CheckIsAdminState.IsErrorCreateToken:

                        context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                            CreateErrorOperation(messages: new string[] { "هناك مشكلة في إنشاء الـصلاحية الدخول" }));
                        return;
                }
                loginUser.Password = _helper.Hash(loginUser.Password);

                var result = await _authUserServices.CheckUserAccess(loginUser.NameOrNumber, loginUser.Password);

                if (result == null)
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "تأكد من البيانات التي تم إدخالها" }));
                    return;
                }

                if (!result.IsActive)
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "لقد تم إيقاف الدخول للنظام بهذا الحساب" }));
                    return;
                }

                if (!result.Permisstions.Any())
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "لقد تم إيقاف الصلاحية الدخول لايمكن تسجيل الدخول" }));
                    return;
                }

                loginUser.UserAuth = result;

            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
