using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models.RegionModel;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.RegionFilters
{
    public class InsertRegionFilter : ActionFilterAttribute
    {

        private readonly IRegionValidationServices _regionValidationServices;
        private readonly HelperUtili _helper;
        public InsertRegionFilter(IRegionValidationServices regionValidationServices, HelperUtili helper)
        {
            _regionValidationServices = regionValidationServices;
            _helper = helper;
        }
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _insertModel);
            if (_insertModel is InsertRegionModel insertModel)
            {
                insertModel.UserId = _helper.GetCurrentUser()?.UserID ?? "AdminSystem";
                if (await _regionValidationServices.CheckRegionDataExists(insertModel.Name, insertModel.RegionNo))
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
