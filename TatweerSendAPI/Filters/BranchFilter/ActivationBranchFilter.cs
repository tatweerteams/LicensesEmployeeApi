using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.BranchFilter
{
    public class ActivationBranchFilter : ActionFilterAttribute
    {

        private readonly IBranchValidationServices _branchValidation;
        public ActivationBranchFilter(IBranchValidationServices branchValidation)
        {
            _branchValidation = branchValidation;
        }
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("branchId", out var _branchId);
            if (!param)
            {

                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "لم يتم إرسال رقم التعريف الفرع" }));
                return;
            }

            if (_branchId is string branchId && !await _branchValidation.CheckIsExistBranchId(branchId))
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                     CreateErrorOperation(messages: new string[] { "بيانات الفرع تم إلغاءها من قبل مستخدم أخر" }));
                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
