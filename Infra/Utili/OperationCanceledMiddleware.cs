using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infra.Utili
{
    public class OperationCanceledMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<OperationCanceledMiddleware> _logger;
        public OperationCanceledMiddleware(
            RequestDelegate next,
            ILogger<OperationCanceledMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Request was cancelled");
                //context.Response.StatusCode = 409;
            }
        }
    }
}
