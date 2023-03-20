using CollactionData.Models.RoleModel;
using IdentityServices.ValidationServicess;
using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityAPI.Filters.RoleFilter
{
    public class InsertRoleFilter : ActionFilterAttribute
    {
        private readonly IRoleValidationServices _validationServices;
        private readonly HelperUtili _helper;
        public InsertRoleFilter(IRoleValidationServices validationServices, HelperUtili helper)
        {
            _validationServices = validationServices;
            _helper = helper;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _insertModel);
            if (_insertModel is InsertRoleModel insertModel)
            {
                var currentUser = _helper.GetCurrentUser();

                insertModel.UserId = "UserId_Test";
                if (await _validationServices.CheckNameExists(insertModel.Name))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات الدور موجودة مسبقا" }));
                    return;
                }

            }
            await base.OnActionExecutionAsync(context, next);
        }

    }
}
