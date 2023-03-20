using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.BranchFilter
{
    public class ActivationBranchWorkTimeFilter : ActionFilterAttribute
    {
        private readonly IBranchValidationServices _branchValidation;
        public ActivationBranchWorkTimeFilter(IBranchValidationServices branchValidation)
        {
            _branchValidation = branchValidation;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("branchWorkTimeId", out var _branchWorkTimeId);
            if (!param)
            {

                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "لم يتم إرسال رقم التعريف الفرع" }));
                return;
            }

            if (_branchWorkTimeId is string branchWorkTimeId && !await _branchValidation.CheckBranchWorkTimeExists(branchWorkTimeId))
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                     CreateErrorOperation(messages: new string[] { "بيانات الفرع تم إلغاءها من قبل مستخدم أخر" }));
                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
