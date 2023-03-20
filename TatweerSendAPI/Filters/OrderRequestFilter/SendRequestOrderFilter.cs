using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.OrderRequestFilter
{
    public class SendRequestOrderFilter : ActionFilterAttribute
    {
        private readonly IOrderRequestValidationServices _validationServices;
        private readonly HelperUtili _helper;
        public SendRequestOrderFilter(IOrderRequestValidationServices validationServices, HelperUtili helper)
        {
            _validationServices = validationServices;
            _helper = helper;
        }


        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
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
                var userId = _helper.GetCurrentUser()?.UserID ?? "1";
                if (!await _validationServices.CheckOrderRequestExists(orderRequestId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "بيانات الطلبية لقد تم إلغاءها" }));
                    return;
                }


                if (!await _validationServices.CheckRequestOrder(userId, orderRequestId: orderRequestId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "لقد تم إرسال هذه الطلبية مسبقا" }));
                    return;
                }

                var minRequestBranch = await _validationServices.CheckMinOrderItemInRequest(orderRequestId);



                if (minRequestBranch != 0)
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { $" هذه الطلبية لايمكن إرسالها يجب ان تحتوي {minRequestBranch} حسابات علي الأقل" }));
                    return;
                }





            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
