using CollactionData.Models.Users;
using IdentityServices.ValidationServicess;
using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityAPI.Filters.UserFilters
{
    public class UpdateUserFilter : ActionFilterAttribute
    {
        private readonly IUserValidationServices _validationServices;
        public UpdateUserFilter(IUserValidationServices validationServices)
        {
            _validationServices = validationServices;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _updateModel);
            if (_updateModel is UpdateUserModel updateModel)
            {
                if (!await _validationServices.CheckIsExists(updateModel.Id))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات المستخدم غير موجود" }));
                    return;
                }

                if (await _validationServices.CheckUserExists(updateModel.Id, updateModel.EmployeeNumber,
                    updateModel.PhoneNumber, updateModel.Email))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات المستخدم موجودة مسبقا" }));
                    return;
                }

            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
