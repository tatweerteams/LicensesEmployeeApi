using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.AccountFilter
{
    public class DeleteAccountFilter : ActionFilterAttribute
    {
        private readonly IAccountValidationServices _accountValidationServices;

        public DeleteAccountFilter( IAccountValidationServices accountValidationServices )
        {
            _accountValidationServices = accountValidationServices;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("id", out var _id);
            if (!param)
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "لم يتم إرسال ر.ت للحساب" }));
                return;
            }

            if (_id is string id)
            {

                if (!await _accountValidationServices.IsAccountExistGetById(id))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات الحساب تم حذفها من قبل مستخدم أخر" }));
                    return;
                }
                if (await _accountValidationServices.IsAccountHasAnyOrder(id))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "لا يمكن حذف حساب تم طلب دفتر صكوك له" }));
                    return;
                }

                //TODO : later check if this account has ordered a checkbook or not
                //UNDONE: testing
                //HACK: test
                //dont delete account has a row in OrderRequest in Db

            }

            await base.OnActionExecutionAsync(context, next);

        }
    }
}
