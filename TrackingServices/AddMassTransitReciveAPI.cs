using Infra;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GenerateIdentityServices
{
    public static class AddMassTransitReciveAPI
    {
        public static IServiceCollection AddMassTransitReciveService(this IServiceCollection services)
        {
            HostBusOptions mqOptions = null;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                services.Configure<HostBusOptions>(configuration.GetSection("BusOptions"));
                mqOptions = configuration.GetOptions<HostBusOptions>("BusOptions");
            }

            services.AddMassTransit(provider =>
            {

                provider.AddBus(rabbit => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(
                    mqOptions.RabbitMqAddress,
                    h =>
                    {
                        h.Username(mqOptions.RabbitMqUserName);
                        h.Password(mqOptions.RabbitMqPassword);
                    });

                }));


            });

            services.AddOptions<MassTransitHostOptions>().Configure(options =>
            {


                options.WaitUntilStarted = true;

                options.StartTimeout = TimeSpan.FromSeconds(10);

                options.StopTimeout = TimeSpan.FromSeconds(30);
            });




            return services;
        }
    }
}
