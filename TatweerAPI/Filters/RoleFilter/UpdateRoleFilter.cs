using CollactionData.Models.RoleModel;
using IdentityServices.ValidationServicess;
using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityAPI.Filters.RoleFilter
{
    public class UpdateRoleFilter : ActionFilterAttribute
    {
        private readonly IRoleValidationServices _validationServices;
        public UpdateRoleFilter(IRoleValidationServices validationServices)
        {
            _validationServices = validationServices;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _updateModel);
            if (_updateModel is UpdateRoleModel updateModel)
            {

                if (!await _validationServices.CheckIsExists(updateModel.Id))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات الدور غير موجودة " }));
                    return;
                }
                if (await _validationServices.CheckNameExists(updateModel.Id, updateModel.Name))
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
