using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models.RegionModel;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.RegionFilters
{
    public class UpdateRegionFilter : ActionFilterAttribute
    {

        private readonly IRegionValidationServices _regionValidationServices;
        public UpdateRegionFilter(IRegionValidationServices regionValidationServices)
        {
            _regionValidationServices = regionValidationServices;
        }
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _modelData);
            if (_modelData is UpdateRegionModel modelData)
            {
                if (!await _regionValidationServices.CheckRegionDelete(modelData.RegionId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات المنطقة تم حذفها من قبل مستخدم أخر" }));
                    return;
                }

                if (await _regionValidationServices.CheckRegionDataExists(modelData.RegionId, modelData.Name, modelData.RegionNo))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات المنطقة موجودة مسبقا" }));
                    return;
                }
            }

            await base.OnActionExecutionAsync(context, next);



        }
    }
}
