

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Infra.Utili
{
    public class EventHandlerUtili : JwtBearerEvents
    {



        public async override Task TokenValidated(TokenValidatedContext context)
        {
            await base.TokenValidated(context);
        }

        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            context.Response.OnStarting(async () =>
            {

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status200OK;
                var result = new
                {
                    Messages = "انتهت صلاحية الدخول",
                };
                var resultOperation = ResultOperationDTO<string>.CreateErrorOperation(messages: new string[] {
                result.Messages
            }, stateResult: StateResult.UnAuth);

                var json = JsonConvert.SerializeObject(resultOperation, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });

                await context.Response.WriteAsync(json);


            });

            return Task.CompletedTask;


        }

        public override Task Forbidden(ForbiddenContext context)
        {

            context.Response.OnStarting(async () =>
            {

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status200OK;
                var result = new
                {
                    Messages = "ليس لديك إذن للدخول",
                };
                var resultOperation = ResultOperationDTO<string>.CreateErrorOperation(messages: new string[] {
                result.Messages
            }, stateResult: StateResult.UnAuth);

                var json = JsonConvert.SerializeObject(resultOperation, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });

                await context.Response.WriteAsync(json);


            });

            return Task.CompletedTask;
        }

        public override Task Challenge(JwtBearerChallengeContext context)
        {
            context.HandleResponse();
            string token;
            if (!TryRetrieveToken(context.Request, out token))
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status200OK;
                var result = new
                {
                    Messages = "لا تملك صلاحية الدخول أو انتهت صلاحية الدخول",
                };
                var resultOperation = ResultOperationDTO<string>.CreateErrorOperation(messages: new string[] {
                result.Messages
                }, stateResult: StateResult.UnAuth);
                var json = JsonConvert.SerializeObject(resultOperation, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
                context.Response.WriteAsync(json);
            }
            return Task.CompletedTask;
        }

        public override Task MessageReceived(MessageReceivedContext context)
        {
            return base.MessageReceived(context);
        }

        private bool TryRetrieveToken(HttpRequest request, out string token)
        {
            token = null;
            Microsoft.Extensions.Primitives.StringValues authzHeaders;
            if (!request.Headers.TryGetValue("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }


            var bearerToken = authzHeaders.ElementAt(0);

            if (bearerToken == "Bearer undefined") return false;

            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

    }
}

