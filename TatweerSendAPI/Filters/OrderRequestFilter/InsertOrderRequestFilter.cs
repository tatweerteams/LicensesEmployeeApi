using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models.OrderRequestModels;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.OrderRequestFilter
{
    public class InsertOrderRequestFilter : ActionFilterAttribute
    {
        private readonly HelperUtili _helper;
        private readonly IOrderRequestValidationServices _orderRequestValidation;
        public InsertOrderRequestFilter(HelperUtili helper, IOrderRequestValidationServices orderRequestValidation)
        {
            _helper = helper;
            _orderRequestValidation = orderRequestValidation;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _insertModel);
            if (_insertModel is InsertOrderRequestModel insertModel)
            {
                var currentUser = _helper.GetCurrentUser();
                insertModel.UserId = currentUser?.UserID ?? "AdminSystem";
                insertModel.EmployeeNo = currentUser?.EmployeeNumber ?? "000";
                insertModel.BranchId = insertModel.BranchId ?? _helper.GetCurrentUser()?.BranchId;


                if (!await _orderRequestValidation.CheckBranchCanRequestOrder(insertModel.BranchId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "لايمكن طلب طلبية  لقد تم إيقاف الطلبات لهذا الفرع" }));
                    return;
                }

                if (await _orderRequestValidation.CheckRequestOrder(insertModel.UserId, branchId: insertModel.BranchId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "لايمكن طلب طلبية أخرى يجب إكمال إرسال الطلبية أولا" }));
                    return;
                }

                if (await _orderRequestValidation.CheckCountRequestOrderByBranchId(insertModel.BranchId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "لقد تم الوصول للحد الأعلى للطلبات لهذا اليوم" }));
                    return;
                }



            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
