using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.BankFilter
{
    public class UpdateBankFilter : ActionFilterAttribute
    {
        private readonly IBankValidationServices _bankValidationServices;
        public UpdateBankFilter(IBankValidationServices bankValidationServices)
        {
            _bankValidationServices = bankValidationServices;
        }
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _updateModel);
            if (_updateModel is UpdateBankModel updateModel)
            {
                if (!await _bankValidationServices.CheckBankIsExists(updateModel.Id))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات المصرف تم حذفها من قبل مستخدم أخر" }));
                    return;
                }

                if (await _bankValidationServices.CheckBankDataExists(updateModel.Id, updateModel.Name, updateModel.BankNo))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات المصرف موجودة مسبقا" }));
                    return;
                }
            }

            await base.OnActionExecutionAsync(context, next);



        }
    }
}
