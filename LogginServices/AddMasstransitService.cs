using Infra;
using LogginServices.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedTatweerSendData.DTOs;

namespace TatweerLogginServices
{
    public static class AddMasstransitService
    {
        public static IServiceCollection AddMassTransitLogginAPI(this IServiceCollection services)
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
                provider.AddConsumer<LogginDataEventConsumer>();
                provider.AddBus(rabbit => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(
                    mqOptions.RabbitMqAddress,
                    h =>
                    {
                        h.Username(mqOptions.RabbitMqUserName);
                        h.Password(mqOptions.RabbitMqPassword);
                    });

                    cfg.ReceiveEndpoint($"{QueueNames.LogginDataEventQueue}", ep =>
                    {
                        ep.PrefetchCount = 30;
                        ep.UseMessageRetry(r => r.Incremental(4, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(20)));
                        ep.ConfigureConsumer<LogginDataEventConsumer>(rabbit);
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
