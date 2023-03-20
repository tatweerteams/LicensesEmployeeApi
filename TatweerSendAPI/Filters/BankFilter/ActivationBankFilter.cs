using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.BankFilter
{
    public class ActivationBankFilter : ActionFilterAttribute
    {

        private readonly IBankValidationServices _bankValidation;
        public ActivationBankFilter(IBankValidationServices bankValidation)
        {
            _bankValidation = bankValidation;
        }
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("bankId", out var _bankIdId);
            if (!param)
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "لم يتم إرسال رقم التعريف المصرف" }));
                return;
            }

            if (_bankIdId is string bankId && !await _bankValidation.CheckBankIsExists(bankId))
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                     CreateErrorOperation(messages: new string[] { "بيانات المصرف تم إلغاءها من قبل مستخدم اخر" }));
                return;
            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
