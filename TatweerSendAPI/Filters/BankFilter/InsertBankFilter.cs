using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.BankFilter
{
    public class InsertBankFilter : ActionFilterAttribute
    {
        private readonly IBankValidationServices _bankValidationServices;
        public InsertBankFilter(IBankValidationServices bankValidationServices)
        {
            _bankValidationServices = bankValidationServices;
        }
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _insertModel);
            if (_insertModel is InsertBankModel insertModel)
            {
                insertModel.UserId = "UserId_Test";
                if (await _bankValidationServices.CheckBankDataExists(insertModel.Name, insertModel.BankNo))
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
