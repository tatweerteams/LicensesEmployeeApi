using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models.Accounts;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.AccountFilter;

public class UpdateAccountFilter : ActionFilterAttribute
{
    private readonly IAccountValidationServices _accountValidationServices;
    private readonly IBranchValidationServices _branchValidationServices;

    public UpdateAccountFilter(
    IAccountValidationServices accountValidationServices,
    IBranchValidationServices branchValidationServices
    )
    {
        _accountValidationServices = accountValidationServices;
        _branchValidationServices = branchValidationServices;
    }

    public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var param = context.ActionArguments.TryGetValue("model", out var _insertModel);
        if (_insertModel is UpdateAccountModel insertModel)
        {
            insertModel.UserId = "test-user";

            if (!await _branchValidationServices.CheckIsExistBranchId(insertModel.BranchId))
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                    CreateErrorOperation(messages: new string[] { "الرجاء اختيار الفرع" }));
                return;
            }

            if (!await _accountValidationServices.IsAccountExistGetById(insertModel.Id))
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                    CreateErrorOperation(messages: new string[] { "هذا الحساب غير موجود" }));
                return;
            }
            
            if (await _accountValidationServices.IsAccountBelongToAnotherPerson(insertModel.Id, insertModel.BranchId, insertModel.AccountNo))
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                    CreateErrorOperation(messages: new string[] { " رقم الحساب هذا مستخدم من قبل عميل اخر" }));
                return;
            }
            if (!await _accountValidationServices.IsValidAccountInputToUpdate(insertModel.Id, insertModel.BranchId, insertModel.AccountNo))
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                    CreateErrorOperation(messages: new string[] { " يوجد خطأ في رقم الحساب " }));
                return;
            }

        }
        await base.OnActionExecutionAsync(context, next);
    }

}
