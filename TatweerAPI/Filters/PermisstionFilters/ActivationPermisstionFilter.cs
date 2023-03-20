using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityAPI.Filters.PermisstionFilters
{
    public class ActivationPermisstionFilter : ActionFilterAttribute
    {
        public ActivationPermisstionFilter()
        {

        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("id", out var _bankIdId);
            if (!param)
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "لم يتم إرسال رقم التعريف الصلاحية" }));
                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
