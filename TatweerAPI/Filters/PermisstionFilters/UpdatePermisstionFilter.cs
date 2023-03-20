using CollactionData.Models.PermisstionModel;
using IdentityServices.ValidationServicess;
using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityAPI.Filters.PermisstionFilters
{
    public class UpdatePermisstionFilter : ActionFilterAttribute
    {
        private readonly IPermisstionValidationServices _validationServices;
        public UpdatePermisstionFilter(IPermisstionValidationServices validationServices)
        {
            _validationServices = validationServices;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _updateModel);
            if (_updateModel is UpdatePermisstionModel updateModel)
            {

                if (await _validationServices.CheckName(updateModel.Id, updateModel.Name))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات الصلاحية موجودة مسبقا" }));
                    return;
                }

            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}