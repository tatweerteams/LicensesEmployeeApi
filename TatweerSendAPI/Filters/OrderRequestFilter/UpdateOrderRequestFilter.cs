using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models.OrderRequestModels;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.OrderRequestFilter
{
    public class UpdateOrderRequestFilter : ActionFilterAttribute
    {
        private readonly IOrderRequestValidationServices _orderRequestValidation;
        private readonly HelperUtili _helper;
        public UpdateOrderRequestFilter(IOrderRequestValidationServices orderRequestValidation,
            HelperUtili helper)
        {
            _orderRequestValidation = orderRequestValidation;
            _helper = helper;
        }


        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _updateModel);

            if (_updateModel is UpdateOrderRequestModel updateModel)
            {
                var userId = _helper.GetCurrentUser()?.UserID ?? "1";
                var branchIdProp = "91431f20-abb9-4e6f-a871-f1724ba2e7dd";


                updateModel.BranchId = string.IsNullOrEmpty(updateModel.BranchId) ?
                   _helper.GetCurrentUser()?.BranchNumber ?? branchIdProp :
                   updateModel.BranchId;

                if (!await _orderRequestValidation.CheckOrderRequestExists(updateModel.Id))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات الطلبية لقد تم إلغاءها" }));
                    return;
                }

                if (await _orderRequestValidation.CheckRequestOrderItems(updateModel.Id))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "هذه الطلبية تحتوي علي حسابات , لايمكن تعديلها" }));
                    return;
                }

                if (!await _orderRequestValidation.CheckRequestOrder(userId, orderRequestId: updateModel.Id))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "لقد تم إرسال هذه الطلبية لايمكن تعديل هذه الطلبية" }));
                    return;
                }

            }

            await base.OnActionExecutionAsync(context, next);

        }



    }
}
