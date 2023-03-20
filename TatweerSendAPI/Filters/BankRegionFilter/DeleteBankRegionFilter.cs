using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.BankRegionFilter
{
    public class DeleteBankRegionFilter : ActionFilterAttribute
    {
        private readonly IBankRegionValidationServices _bankRegionValidationServices;
        public DeleteBankRegionFilter(IBankRegionValidationServices bankRegionValidationServices)
        {
            _bankRegionValidationServices = bankRegionValidationServices;
        }
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("bankRegionId", out var _bankRegionId);
            if (!param)
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[]
                       { "لم يتم إرسال رقم التعريف المنطقة" }));
                return;
            }
            if (_bankRegionId is string bankRegionId)
            {
                if (!await _bankRegionValidationServices.CheckExistBankRegion(bankRegionId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[]
                        { "تم إلغاء المنطقة من قبل مستخدم اخر" }));
                    return;
                }

                if (await _bankRegionValidationServices.CanDeleteBankRegion(bankRegionId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[]
                        { "هذه المنطقة لديها فروع لايمكن إلغاؤها" }));
                    return;
                }

            }

            await base.OnActionExecutionAsync(context, next);



        }
    }
}
