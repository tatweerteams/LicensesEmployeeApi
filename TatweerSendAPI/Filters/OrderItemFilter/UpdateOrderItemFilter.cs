using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models.OrderItemModel;
using TatweerSendServices.ExtensionServices;
using TatweerSendServices.services;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.OrderItemFilter
{
    public class UpdateOrderItemFilter : ActionFilterAttribute
    {
        private readonly HelperUtili _helper;
        private readonly IOrderItemValidationServices _itemValidationServices;
        private readonly IOrderRequestServices _orderRequestServices;
        public UpdateOrderItemFilter(HelperUtili helper,
            IOrderItemValidationServices itemValidationServices, IOrderRequestServices orderRequestServices)
        {
            _helper = helper;
            _itemValidationServices = itemValidationServices;
            _orderRequestServices = orderRequestServices;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _updateModel);
            if (_updateModel is UpdateOrderItemModel updateModel)
            {

                var result = await _orderRequestServices.GetById(updateModel.OrderRequestId);

                if (result == null)
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "لقد تم إلغاء هذه الطلبية" }));
                    return;
                }




                if (!await _itemValidationServices.CheckIsExists(updateModel.Id))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "هذا الحساب غير موجود في القائمة" }));
                    return;
                }

                var isIndividualRequest = result.OrderRequestType.OrderRequestTypeIndividualValidation();

                if (isIndividualRequest && !await _itemValidationServices.CheckIndividualQuentityOfDay(updateModel.AccountId, updateModel.CountChekBook))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "لايمكن إجتياز الحد الأعلي لعدد الدفاتر في اليوم" }));
                    return;
                }


            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
