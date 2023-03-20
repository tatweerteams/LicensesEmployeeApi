using IdentityServices.ValidationServicess;
using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityAPI.Filters.RoleFilter
{
    public class DeleteRoleFilter : ActionFilterAttribute
    {
        private readonly IRoleValidationServices _validationServices;
        public DeleteRoleFilter(IRoleValidationServices validationServices)
        {
            _validationServices = validationServices;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("roleId", out var _roleId);
            if (!param)
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "لم يتم إرسال رقم التعريف الدور" }));
                return;
            }
            if (_roleId is string roleId)
            {

                if (!await _validationServices.CheckIsExists(roleId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات الدور غير موجودة " }));
                    return;
                }
                if (await _validationServices.CheckCanDelete(roleId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "لايمكن إلغاء بيانات الدور" }));
                    return;
                }

            }
            await base.OnActionExecutionAsync(context, next);
        }

    }
}
