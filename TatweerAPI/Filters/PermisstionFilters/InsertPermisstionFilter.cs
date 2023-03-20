using CollactionData.Models.PermisstionModel;
using IdentityServices.ValidationServicess;
using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityAPI.Filters.PermisstionFilters
{
    public class InsertPermisstionFilter : ActionFilterAttribute
    {
        private readonly IPermisstionValidationServices _validationServices;
        public InsertPermisstionFilter(IPermisstionValidationServices validationServices)
        {
            _validationServices = validationServices;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _insertModel);
            if (_insertModel is InsertPermisstionModel insertModel)
            {

                if (await _validationServices.CheckName(insertModel.Name))
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
