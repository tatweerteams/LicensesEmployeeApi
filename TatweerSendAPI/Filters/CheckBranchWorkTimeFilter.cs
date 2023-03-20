using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TatweerSendServices.services;

namespace TatweerSendAPI.Filters
{
    public class CheckBranchWorkTimeFilter : ActionFilterAttribute
    {
        private readonly HelperUtili _helper;
        private readonly IBranchServices _branchServices;
        public CheckBranchWorkTimeFilter(HelperUtili helper, IBranchServices branchServices)
        {
            _helper = helper;
            _branchServices = branchServices;
        }



        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var tody = DateTime.Now;

            var cuurentUser = _helper.GetCurrentUser();

            if (cuurentUser.UserType.Equals(UserTypeState.SuperAdmin))
            {
                await base.OnActionExecutionAsync(context, next);
                return;
            }

            var result = await _branchServices.GetWorkTimeByBranchIdAndDay(cuurentUser.BranchId, DateTime.Now.DayOfWeek);
            if (result == null)
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                       CreateErrorOperation(
                        messages: new string[] { "لايمكن الدخول إنتهى وقت الدوام" },
                        stateResult: StateResult.WorkTimeOut
                       ));
                return;
            }


            if (!result.IsActive)
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                      CreateErrorOperation(
                       messages: new string[] { "ليس لديك صلاحية دخول اليوم" },
                       stateResult: StateResult.WorkTimeOut
                      ));
                return;
            }

            var timeStart = tody - DateTime.Parse(result.TimeStart);
            var timeEnd = tody - DateTime.Parse(result.TimeEnd);


            if (timeStart.TotalMinutes < 0)
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                      CreateErrorOperation(
                       messages: new string[] { "وقت دوام العمل لم يبدأ بعد" },
                       stateResult: StateResult.WorkTimeOut
                      ));
                return;
            }

            if (timeEnd.TotalMinutes > 0)
            {
                context.Result = new OkObjectResult(ResultOperationDTO<bool>.
                      CreateErrorOperation(
                       messages: new string[] { "إنتهى وقت دوام العمل" },
                       stateResult: StateResult.WorkTimeOut
                      ));
                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
