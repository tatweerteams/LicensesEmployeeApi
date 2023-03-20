using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models.Accounts;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.AccountFilter
{
    public class InsertAccountFilter : ActionFilterAttribute
    {

        private readonly IAccountValidationServices _accountValidationServices;
        private readonly IBranchValidationServices _branchValidationServices;
        private readonly HelperUtili _helper;

        public InsertAccountFilter (
            IAccountValidationServices accountValidationServices,
            IBranchValidationServices branchValidationServices,
            HelperUtili helper
        )
        {
            _accountValidationServices = accountValidationServices;
            _branchValidationServices = branchValidationServices; 
            _helper = helper;

        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _insertModel);
            if (_insertModel is InsertAccountModel insertModel)
            {
                if (await _branchValidationServices.CheckIsExistBranchId(insertModel.BranchId) == false)
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "الرجاء اختيار الفرع" }));
                    return;
                }

                //insertModel.UserId = "test-user1";
                var currentUser = _helper.GetCurrentUser();
                insertModel.UserId = currentUser.UserID ?? "AdminSystem";

                if (await _accountValidationServices.IsAccountNumberExist(insertModel.AccountNo))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "هذا الحساب تم ادراجه مسبقا" }));
                    return;
                }
                if (await _accountValidationServices.IsPhoneNumberExist(insertModel.PhoneNumber))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "رقم الهاتف تم ادراجه مسبقا" }));
                    return;
                }

            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
