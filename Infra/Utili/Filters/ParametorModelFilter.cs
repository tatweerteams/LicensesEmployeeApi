using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
 
namespace Infra.Utili.Filters
{
    public class ParametorModelFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<ErrorProperty> errorProperties = new List<ErrorProperty>();
                var result = context.ModelState.Where(w => w.Value!.Errors.Count > 0).Distinct()
                    .Select(s => new ErrorProperty
                    { 
                        Key = s.Key,
                        Error = s.Value.Errors.FirstOrDefault().ErrorMessage
                    }).ToList();
                
                context.Result = new OkObjectResult(new ResponseError(result, StateResult.empty));

                if (result.All(a => a.Key == ""))
                {
                    context.Result = new OkObjectResult(new string[] { "يجب ارسال البيانات المطلوبة" });

                }
            }
           
            base.OnResultExecuting(context);
        }

    }

    class ResponseError
    {
        public List<ErrorProperty> Messages { get; set; }
        public StateResult State { get; set; }
        public ResponseError(List<ErrorProperty> Messages, StateResult State)
        {
            this.Messages = Messages;
            this.State = State;
        }

    }
    class ErrorProperty
    {
        public string? Key { get; set; }
        public string? Error { get; set; }
    }
}
