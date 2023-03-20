using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models.BranchModels;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.BranchFilter
{
    public class InsertBranchFilter : ActionFilterAttribute
    {
        private readonly IBranchValidationServices _branchValidation;
        private readonly HelperUtili _helper;
        public InsertBranchFilter(IBranchValidationServices branchValidation, HelperUtili helper)
        {
            _branchValidation = branchValidation;
            _helper = helper;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _insertModel);
            if (_insertModel is InsertBranchModel insertModel)
            {
                var currentUser = _helper.GetCurrentUser();
                insertModel.BranchRegionId = insertModel.BranchRegionId ?? currentUser?.RegionId;
                insertModel.UserId = currentUser.UserID ?? "AdminSystem";
                if (await _branchValidation.
                    IsExistsData(insertModel.Name, insertModel.BranchNo, insertModel.BranchRegionId))
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
