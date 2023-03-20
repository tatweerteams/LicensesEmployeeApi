using infra.rabbitMq.rabbitMqServices;
using Infra.Services.rabbitMq;
using Microsoft.Extensions.DependencyInjection;
using SendEventBus.PublishEvents;

namespace SendEventBus
{
    public static class DependencieInjectionPublishEvent
    {
        public static IServiceCollection AddEventPublishDI(this IServiceCollection services)
        {


            services.AddScoped(typeof(ISendNotifyServices<>), typeof(SendNotifyServices<>));

            services.AddScoped<TrackingOrderEventPublish>();
            services.AddScoped<GenerateIdentityPublish>();
            services.AddScoped<GenerateSerialNumberPublish>();


            return services;
        }
    }
}
