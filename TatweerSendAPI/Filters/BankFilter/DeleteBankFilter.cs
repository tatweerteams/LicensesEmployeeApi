using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.BankFilter
{
    public class DeleteBankFilter : ActionFilterAttribute
    {

        private readonly IBankValidationServices _bankValidationServices;
        private readonly IBankRegionValidationServices _bankRegionValidationServices;
        public DeleteBankFilter(
            IBankValidationServices bankValidationServices,
            IBankRegionValidationServices bankRegionValidationServices)
        {
            _bankValidationServices = bankValidationServices;
            _bankRegionValidationServices = bankRegionValidationServices;
        }
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("bankId", out var _bankId);
            if (!param)
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "لم يتم إرسال رقم التعريف المصرف" }));
                return;
            }
            if (_bankId is string bankId)
            {

                if (!await _bankValidationServices.CheckBankIsExists(bankId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات المصرف تم حذفها من قبل مستخدم أخر" }));
                    return;
                }

                if (await _bankRegionValidationServices.CanNotDeleteBank(bankId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "هذا المصرف يحتوي علي فروع لايمكن حذفه" }));
                    return;
                }

            }

            await base.OnActionExecutionAsync(context, next);



        }
    }
}
