using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.RegionFilters
{
    public class DeleteRegionFilter : ActionFilterAttribute
    {

        private readonly IRegionValidationServices _regionValidationServices;
        private readonly IBankRegionValidationServices _bankRegionValidationServices;
        public DeleteRegionFilter(
            IRegionValidationServices regionValidationServices,
            IBankRegionValidationServices bankRegionValidationServices
            )
        {
            _regionValidationServices = regionValidationServices;
            _bankRegionValidationServices = bankRegionValidationServices;
        }
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("regionId", out var _regionId);
            if (!param)
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "لم يتم إرسال رقم التعريف المنطقة" }));
                return;
            }
            if (_regionId is string regionId)
            {

                if (!await _regionValidationServices.CheckRegionDelete(regionId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات المنطقة تم حذفها من قبل مستخدم أخر" }));
                    return;
                }

                if (await _bankRegionValidationServices.CanDeleteRegion(regionId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "لايمكن حذف المنطقة يجب إلغاؤها من المصرف أولا" }));
                    return;
                }

            }

            await base.OnActionExecutionAsync(context, next);



        }
    }
}
