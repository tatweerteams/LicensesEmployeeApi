﻿using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.OrderItemFilter
{
    public class DeleteOrderItemFilter : ActionFilterAttribute
    {
        private readonly HelperUtili _helper;
        private readonly IOrderItemValidationServices _itemValidationServices;
        public DeleteOrderItemFilter(HelperUtili helper,
            IOrderItemValidationServices itemValidationServices)
        {
            _helper = helper;
            _itemValidationServices = itemValidationServices;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("orderItemId", out var _orderItemId);

            if (!param)
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(messages: new string[] { "لم يتم إرسال رقم التعريف الحساب" }));
                return;
            }

            if (_orderItemId is string orderItemId)
            {

                if (!await _itemValidationServices.CheckIsExists(orderItemId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "هذا الحساب غير موجود في القائمة" }));
                    return;
                }

            }
            await base.OnActionExecutionAsync(context, next);
        }

    }
}
