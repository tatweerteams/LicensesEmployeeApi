
using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FilterAttributeWebAPI.Common
{
    public class ExceptionHandlerUtili : ExceptionFilterAttribute, IExceptionFilter
    {
        public ExceptionHandlerUtili()
        {

        }

        public async override Task OnExceptionAsync(ExceptionContext context)
        {
            var resultError = "هناك مشكلة في النظام";
            await Task.FromResult(true);
            var inner_Exception = "No Inner Exception";
            if (context.Exception.InnerException != null)
            {
                inner_Exception = context.Exception.InnerException.Message;
            }

            if (context.Exception is ApplicationEx ex)
                resultError = ex.Error;


            var message =
                "\n\n\n ***********************************************" + DateTime.Now.ToString() + "*****************************************************" + "\n\n\n" +
                "Status Code                    ====> \t" + 400 + "\n\n\n" +
                "Path Error                     ====> \t" + context.ActionDescriptor.DisplayName + "\n\n\n" +
                "Type Execut Exception          ====> \t" + "ExecuteException" + "\n\n\n" +
                "Exception Masseges             ====> \t" + context.Exception.Message + "\n\n\n" +
                "inner Exception                ====> \t" + inner_Exception + "\n\n\n" +
                "Date Exception                 ====> \t" + DateTime.Now.ToString() + "\n\n\n" +
                "Source Exception               ====> \t" + context.Exception.StackTrace + "\n\n\n";

            //_logger.LogError(message);

            //await LogDataInFile(message);

            var resultOperation = ResultOperationDTO<object>.CreateErrorOperation(messages: new string[] {
                resultError
            }, stateResult: StateResult.ex);

            context.Result = new OkObjectResult(resultOperation);
            await base.OnExceptionAsync(context);
        }
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    var r = _helper.GetCurrentUser();
        //    base.OnActionExecuting(context);
        //}
        //public async void OnException(ExceptionContext actionExecutedContext)
        //{
        //    var resultError = "هناك مشكلة في النظام";
        //    await Task.FromResult(true);
        //    var inner_Exception = "No Inner Exception";
        //    if (actionExecutedContext.Exception.InnerException != null)
        //    {
        //        inner_Exception = actionExecutedContext.Exception.InnerException.Message;
        //    }

        //    if (actionExecutedContext.Exception is ApplicationEx ex)
        //        resultError = ex.Error;


        //    //var result = new
        //    //{
        //    //    Messages = actionExecutedContext.Exception.Message + " " + inner_Exception,

        //    //    ActionError = actionExecutedContext.ActionDescriptor.DisplayName,
        //    //};



        //    var message =
        //        "\n\n\n ***********************************************" + DateTime.Now.ToString() + "*****************************************************" + "\n\n\n" +
        //        "Status Code                    ====> \t" + 400 + "\n\n\n" +
        //        "Path Error                     ====> \t" + actionExecutedContext.ActionDescriptor.DisplayName + "\n\n\n" +
        //        "Type Execut Exception          ====> \t" + "ExecuteException" + "\n\n\n" +
        //        "Exception Masseges             ====> \t" + actionExecutedContext.Exception.Message + "\n\n\n" +
        //        "inner Exception                ====> \t" + inner_Exception + "\n\n\n" +
        //        "Date Exception                 ====> \t" + DateTime.Now.ToString() + "\n\n\n" +
        //        "Source Exception               ====> \t" + actionExecutedContext.Exception.StackTrace + "\n\n\n";

        //    _logger.LogError(message);

        //    //await LogDataInFile(message);

        //    var resultOperation = ResultOperationDTO<object>.CreateErrorOperation(messages: new string[] {
        //        resultError
        //    }, stateResult: StateResult.ex);

        //    actionExecutedContext.Result = new OkObjectResult(resultOperation);
        //}
        private async Task LogDataInFile(string data)
        {
            var pathFull = Path.GetFullPath("~/LoggingFile/");
            var pathFile = pathFull.Replace("~", "");
            var dateNow = "LogException" + DateTime.Now.ToString("yyyy_MM_dd");
            await File.AppendAllTextAsync(pathFile + dateNow + ".txt", data);
        }
    }

    public class ApplicationEx : Exception
    {
        public string Error { get; private set; }
        public ApplicationEx(string Error) : base(Error)
        {
            this.Error = Error;
        }

    }
}