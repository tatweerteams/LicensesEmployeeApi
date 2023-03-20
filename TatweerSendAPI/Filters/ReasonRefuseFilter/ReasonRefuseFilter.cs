using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.ReasonRefuseFilter
{
    public class ReasonRefuseFilter : ActionFilterAttribute
    {

        private readonly IReasonRefuseValidationServices _validationServices;
        public ReasonRefuseFilter(IReasonRefuseValidationServices validationServices)
        {
            _validationServices = validationServices;
        }


        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("reasonRefuseId", out var _reasonRefuseId);
            if (_reasonRefuseId is int reasonRefuseId)
            {

                if (!await _validationServices.CheckIsExists(reasonRefuseId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "بيانات السبب الرفض لقد تم إلغاءها" }));
                    return;
                }

            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
