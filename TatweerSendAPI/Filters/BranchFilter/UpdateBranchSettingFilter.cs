using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models.BranchModels;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.BranchFilter
{
    public class UpdateBranchSettingFilter : ActionFilterAttribute
    {
        private readonly IBranchValidationServices _branchValidation;
        public UpdateBranchSettingFilter(IBranchValidationServices branchValidation)
        {
            _branchValidation = branchValidation;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _updateModel);

            if (_updateModel is UpdateBranchSettingModel updateModel)
            {
                if (!await _branchValidation.CheckBranchSettingIsExist(updateModel.Id))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات الفرع تم حذفها من قبل مستخدم أخر" }));
                    return;
                }

            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
