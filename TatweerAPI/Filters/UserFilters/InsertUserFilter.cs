using CollactionData.Models.Users;
using IdentityServices.ValidationServicess;
using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityAPI.Filters.UserFilters
{
    public class InsertUserFilter : ActionFilterAttribute
    {
        private readonly IUserValidationServices _validationServices;
        public InsertUserFilter(IUserValidationServices validationServices)
        {
            _validationServices = validationServices;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var param = context.ActionArguments.TryGetValue("model", out var _insertModel);
            if (_insertModel is InsertUserModel insertModel)
            {


                if (await _validationServices.CheckUserExists(insertModel.EmployeeNumber, insertModel.PhoneNumber, insertModel.Email))
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
