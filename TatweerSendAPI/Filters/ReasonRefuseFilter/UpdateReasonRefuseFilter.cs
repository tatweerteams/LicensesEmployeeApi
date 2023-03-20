using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models.ReasonRefuseModel;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.ReasonRefuseFilter
{
    public class UpdateReasonRefuseFilter : ActionFilterAttribute
    {

        private readonly IReasonRefuseValidationServices _validationServices;
        private readonly HelperUtili _helper;
        public UpdateReasonRefuseFilter(IReasonRefuseValidationServices validationServices, HelperUtili helper)
        {
            _validationServices = validationServices;
            _helper = helper;
        }


        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _insertModel);
            if (_insertModel is UpdateReasonRefuseModel updateModel)
            {

                if (!await _validationServices.CheckIsExists(updateModel.Id))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "بيانات السبب الرفض لقد تم إلغاءها" }));
                    return;
                }

                if (await _validationServices.CheckMessageExests(updateModel.Id, updateModel.Name))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "بيانات السبب الرفض موجودة مسبقا" }));
                    return;
                }


            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
