using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.OrderRequestFilter
{
    public class DeleteOrderRequestFilter : ActionFilterAttribute
    {

        private readonly IOrderRequestValidationServices _orderRequestValidation;
        private readonly HelperUtili _helper;
        public DeleteOrderRequestFilter(IOrderRequestValidationServices orderRequestValidation,
            HelperUtili helper)
        {
            _orderRequestValidation = orderRequestValidation;
            _helper = helper;
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

                var userId = _helper.GetCurrentUser()?.UserID ?? "1";

                if (!await _orderRequestValidation.CheckRequestOrder(userId, orderRequestId: orderRequestId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "لقد تم إرسال هذه الطلبية لايمكن إلغاء هذه الطلبية" }));
                    return;
                }

            }

            await base.OnActionExecutionAsync(context, next);

        }
    }
}
