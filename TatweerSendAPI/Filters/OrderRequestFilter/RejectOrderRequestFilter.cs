using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.OrderRequestFilter
{
    public class RejectOrderRequestFilter : ActionFilterAttribute
    {

        private readonly IOrderRequestValidationServices _orderRequestValidation;

        public RejectOrderRequestFilter(IOrderRequestValidationServices orderRequestValidation)
        {
            _orderRequestValidation = orderRequestValidation;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("orderRequestId", out var _orderRequestId);
            if (!param)
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "لم يتم إرسال رقم التعريف الطلب" }));
                return;
            }

            if (_orderRequestId is string orderRequestId)
            {

                if (!await _orderRequestValidation.CheckOrderRequestExists(orderRequestId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات الطلبية لقد تم إلغاءها" }));
                    return;
                }


                if (!await _orderRequestValidation.CheckCanRejectRequest(orderRequestId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "لقد تم قبول أو رفض هذا الطلب " }));
                    return;
                }


            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
