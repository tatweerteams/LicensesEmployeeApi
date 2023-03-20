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
    public class InsertOrderItemFilter : ActionFilterAttribute
    {
        private readonly HelperUtili _helper;
        private readonly IOrderItemValidationServices _itemValidationServices;
        private readonly IOrderRequestServices _orderRequestServices;
        public InsertOrderItemFilter(HelperUtili helper,
            IOrderItemValidationServices itemValidationServices, IOrderRequestServices orderRequestServices)
        {
            _helper = helper;
            _itemValidationServices = itemValidationServices;
            _orderRequestServices = orderRequestServices;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _insertModel);
            if (_insertModel is InsertOrderItemModel insertModel)
            {
                var result = await _orderRequestServices.GetById(insertModel.OrderRequestId);

                if (result == null)
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "لقد تم إلغاء هذه الطلبية" }));
                    return;
                }

                var isIndividualRequest = result.OrderRequestType.OrderRequestTypeIndividualValidation();

                if (await _itemValidationServices.CheckAccountIsExists(insertModel.OrderRequestId, insertModel.AccountId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "هذا الحساب لقد تم إدراجه في الطلب لايمكن إدراجه مرة أخري" }));
                    return;
                }

                if (isIndividualRequest && await _itemValidationServices.CheckAccountCanNotAddOfDay(insertModel.AccountId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات الحساب لقد تم طلب له اليوم لايمكن طلب له مره أخري" }));
                    return;
                }

                if (isIndividualRequest && !await _itemValidationServices.CheckIndividualQuentityOfDay(insertModel.AccountId, insertModel.CountChekBook))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "لايمكن إجتياز الحد الأعلي لعدد الدفاتر في اليوم" }));
                    return;
                }

                if (await _itemValidationServices.MaxItemInRequestOrder(insertModel.OrderRequestId, result.OrderRequestType))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "هذه الطلبية قد وصلت إلى الحد الأعلى للطلبات" }));
                    return;
                }

            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
