using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.BankRegionFilter
{
    public class ActivationBankRegionFilter : ActionFilterAttribute
    {

        private readonly IBankRegionValidationServices _bankRegionValidation;
        public ActivationBankRegionFilter(IBankRegionValidationServices bankRegionValidation)
        {
            _bankRegionValidation = bankRegionValidation;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("bankRegionId", out var _bankRegionId);
            if (!param)
            {

                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "لم يتم إرسال رقم التعريف" }));
                return;
            }

            if (_bankRegionId is string bankRegionId && !await _bankRegionValidation.CheckExistBankRegion(bankRegionId))
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                     CreateErrorOperation(messages: new string[] { "بيانات المنطقة تم إلغاءها من قبل مستخدم اخر" }));
                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
