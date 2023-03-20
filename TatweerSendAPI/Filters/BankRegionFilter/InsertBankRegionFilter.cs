using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.BankRegionFilter
{
    public class InsertBankRegionFilter : ActionFilterAttribute
    {
        private readonly IBankRegionValidationServices _ValidationServices;
        private readonly IRegionValidationServices _RegionValidationServices;
        private readonly HelperUtili _helper;

        public InsertBankRegionFilter(
            IBankRegionValidationServices ValidationServices,
            IRegionValidationServices regionValidationServices,
            HelperUtili helper)
        {
            _ValidationServices = ValidationServices;
            _RegionValidationServices = regionValidationServices;
            _helper = helper;
        }
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _insertModel);
            if (_insertModel is InsertBankRegionModel insertModel)
            {
                var currentUser = _helper.GetCurrentUser();
                insertModel.BankId = insertModel.BankId ?? currentUser?.BankId;
                insertModel.RegionId = insertModel.RegionId ?? currentUser?.RegionId;
                insertModel.UserId = currentUser.UserID ?? "AdminSystem";

                if (!await _RegionValidationServices.CheckRegionDelete(insertModel.RegionId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "بيانات المنطقة غير موجودة" }));
                    return;
                }

                if (await _ValidationServices.ExistData(insertModel.BankId, insertModel.RegionId))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                        CreateErrorOperation(messages: new string[] { "تم إدراج المنطقة للمصرف مسبقا" }));
                    return;
                }
            }

            await base.OnActionExecutionAsync(context, next);



        }
    }
}
