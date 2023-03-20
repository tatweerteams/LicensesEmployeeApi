using IdentityServices.ValidationServicess;
using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityAPI.Filters.UserFilters
{
    public class CheckUserFilter : ActionFilterAttribute
    {
        private readonly IUserValidationServices _validationServices;
        public CheckUserFilter(IUserValidationServices validationServices)
        {
            _validationServices = validationServices;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("userId", out var _userId);

            if (!param)
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "لم يتم إرسال رقم التعريف المستخدم" }));
                return;
            }

            if (_userId is string userId)
            {
                if (!await _validationServices.CheckIsExists(userId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات المستخدم غير موجودة" }));
                    return;
                }
            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
