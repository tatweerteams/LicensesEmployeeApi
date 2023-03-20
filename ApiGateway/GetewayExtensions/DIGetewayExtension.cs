using Infra.Utili;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;

namespace ApiGateway.GetewayExtensions
{
    public static class DIGetewayExtension
    {
        public static IServiceCollection AddGetewayDIExtensionServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<EventHandlerUtili>();
            services.TryAddSingleton<HelperUtili>();

            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            services.AddOcelot(configuration).AddPolly();
            return services;
        }
    }
}
