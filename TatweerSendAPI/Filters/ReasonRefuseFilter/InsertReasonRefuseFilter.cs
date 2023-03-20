using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models.ReasonRefuseModel;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.ReasonRefuseFilter
{
    public class InsertReasonRefuseFilter : ActionFilterAttribute
    {

        private readonly IReasonRefuseValidationServices _validationServices;
        private readonly HelperUtili _helper;
        public InsertReasonRefuseFilter(IReasonRefuseValidationServices validationServices, HelperUtili helper)
        {
            _validationServices = validationServices;
            _helper = helper;
        }


        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _insertModel);
            if (_insertModel is InsertReasonRefuseModel insertModel)
            {
                var rrrr = _helper.GetCurrentUser();
                insertModel.EmployeeNo = "105"; /*_helper.GetCurrentUser().EmployeeNo */
                insertModel.UserId = "1" /*_helper.GetCurrentUser().UserId */;

                if (await _validationServices.CheckMessageExests(insertModel.Name))
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
