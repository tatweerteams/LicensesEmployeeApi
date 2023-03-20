using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models.Accounts;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.AccountFilter
{
    public class InsertListOfAccountsFilter : ActionFilterAttribute
    {
        
        private readonly IAccountValidationServices _accountValidationServices;
        private readonly IBranchValidationServices _branchValidationServices;
        private readonly HelperUtili _helper;

        public InsertListOfAccountsFilter(
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

            if ( _insertModel is InsertAccountsModel model)
            {
                var currentUser = _helper.GetCurrentUser();

                if ( model.insertModel.Count <= 0) { ReturnErrorMessage( context, " خطأ : قائمة الحسابات يجب ان لا تكون فارغة" ); return; }

                var branch = await _accountValidationServices.GetBranchById(model.branchId);

                if ( branch == null ) { ReturnErrorMessage(context, "خطأ في رقم الفرع"); return; }

                if(model.insertModel.Any(account => account.AccountNo.Length < 14))
                { ReturnErrorMessage(context, "خطا في احد الحسابات"); return; }

                if( !model.insertModel.Any(account => account.AccountNo.Substring(0, 3).Equals(branch.BranchNo)))
                { ReturnErrorMessage(context, "خطا في رقم الفرع في احد الحسابات"); return; }

                var accountNos= model.insertModel.Select(s => s.AccountNo).ToList();    

                var dbAccounts = await _accountValidationServices.GetListOfAccounts(accountNos, model.branchId);

                model.insertModel.RemoveAll( pred => dbAccounts.Contains(pred.AccountNo));

                if( !model.insertModel.Any() ) { ReturnErrorMessage(context, " تم ادخال هذه الحسابات مسبقاً  "); return; }

                model.insertModel.All( account =>
                {
                    account.UserId = currentUser.UserID ?? "AdminSystem";
                    account.BranchId = model.branchId;
                    account.InputType = InputAccountTypeState.ImportExcel;
                    account.AccountState = AccountState.IsActive;
                    return true;
                });

            }

            await base.OnActionExecutionAsync(context, next);
        }

        public void ReturnErrorMessage(ActionExecutingContext context, string message)
        {
            context.Result = new OkObjectResult(ResultOperationDTO<bool>.
            CreateErrorOperation(messages: new string[] { message }));
        }

    }
}
