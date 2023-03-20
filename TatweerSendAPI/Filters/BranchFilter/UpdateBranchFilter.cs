using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models.BranchModels;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.BranchFilter
{
    public class UpdateBranchFilter : ActionFilterAttribute
    {
        private readonly IBranchValidationServices _branchValidation;
        private readonly HelperUtili _helper;

        public UpdateBranchFilter(IBranchValidationServices branchValidation, HelperUtili helper)
        {
            _branchValidation = branchValidation;
            _helper = helper;
        }
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _updateModel);
            if (_updateModel is UpdateBranchModel updateModel)
            {

                if (!await _branchValidation.CheckIsExistBranchId(updateModel.Id))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات الفرع تم حذفها من قبل مستخدم أخر" }));
                    return;
                }
                var currentUser = _helper.GetCurrentUser();
                updateModel.BranchRegionId = updateModel.BranchRegionId ?? currentUser?.RegionId;

                if (await _branchValidation.
                    IsExistsData(updateModel.Id, updateModel.Name, updateModel.BranchNo, updateModel.BranchRegionId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات الفرع موجودة مسبقا" }));
                    return;
                }

            }

            await base.OnActionExecutionAsync(context, next);



        }
    }
}
