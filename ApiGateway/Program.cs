using ApiGateway.GetewayExtensions;
using Infra;
using Ocelot.Middleware;
using System.Net;

var builder = WebApplication.CreateBuilder(args);



builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddEndpointsApiExplorer()
    .AddSwagger("Api Geteway")
    .AddCorsPolicy()
    .AddGetewayDIExtensionServices()
.AddJWT();




var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCorsPolicy();
app.UseAuthentication();
app.UseAuthorization();

var configuration = new OcelotPipelineConfiguration
{
    PreAuthorizationMiddleware = async (ctx, next) =>
    {
        var isAuthorization = await CheckAuthExtensions.CheckIsAuthorization(ctx);
        if (isAuthorization)
        {
            await next.Invoke();
        }
        else
        {

            var content = CheckAuthExtensions.GetStringResponse("ليس لديك تخويل للدخول ");
            ctx.Items.UpsertDownstreamResponse(new DownstreamResponse(new HttpResponseMessage()
            {
                Content = content,
                StatusCode = HttpStatusCode.OK
            }));

            return;
        }
    },

    PreAuthenticationMiddleware = async (ctx, next) =>
    {
        var authSuccess = await CheckAuthExtensions.CheckAuthenticated(ctx);

        if (authSuccess)
        {
            await next.Invoke();
        }
        else
        {
            var content = CheckAuthExtensions.GetStringResponse("بجب تسجيل الدخول للنظام");
            ctx.Items.UpsertDownstreamResponse(new DownstreamResponse(new HttpResponseMessage()
            {
                Content = content,
                StatusCode = HttpStatusCode.OK
            }));

            return;
        }

    },

    AuthorizationMiddleware = async (ctx, next) =>
    {
        await next.Invoke();
    },

};

await app.UseOcelot(configuration);


app.Run();