using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models.BranchModels;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.BranchFilter
{
    public class UpdateBranchWorkTimeFilter : ActionFilterAttribute
    {
        private readonly IBranchValidationServices _branchValidation;
        public UpdateBranchWorkTimeFilter(IBranchValidationServices branchValidation)
        {
            _branchValidation = branchValidation;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _model);
            if (!param)
            {

                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "لم يتم إرسال رقم التعريف الفرع" }));
                return;
            }

            if (_model is BranchWorkTimeModel model && !await _branchValidation.CheckBranchWorkTimeExists(model.Id))
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                     CreateErrorOperation(messages: new string[] { "بيانات الفرع تم إلغاءها من قبل مستخدم أخر" }));
                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
