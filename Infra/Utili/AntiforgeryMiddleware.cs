using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace Infra.Utili
{
    public class AntiforgeryMiddleware : IMiddleware
    {
        private readonly IAntiforgery _antiforgery;

        public AntiforgeryMiddleware(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var isGetRequest = string.Equals("GET", context.Request.Method, StringComparison.OrdinalIgnoreCase);
            try
            {
                if (!isGetRequest)
                {
                    //var result = _antiforgery.GetTokens(context);

                    //var test = await _antiforgery.IsRequestValidAsync(context);

                    //await _antiforgery.ValidateRequestAsync(context);
                }

                await next(context);
            }
            catch (Exception ex)
            {

                //throw ex;
            }


        }
    }
}
