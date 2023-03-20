using GenerateIdentityServices.Consumers;
using Infra;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedTatweerSendData.DTOs;

namespace GenerateIdentityServices
{
    public static class AddMassTransitGenerateAPI
    {
        public static IServiceCollection AddMassTransitGenerateService(this IServiceCollection services)
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
                provider.AddConsumer<GenerateIdentityConsumer>();
                provider.AddConsumer<TrackingOrderRequestEventConsumer>();
                provider.AddConsumer<OrderRequestChangeStatusConsumer>();

                provider.AddBus(rabbit => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(
                    mqOptions.RabbitMqAddress,
                    h =>
                    {
                        h.Username(mqOptions.RabbitMqUserName);
                        h.Password(mqOptions.RabbitMqPassword);
                    });

                    cfg.ReceiveEndpoint($"{QueueNames.GenerateIdentityNameQueue}", ep =>
                    {
                        ep.PrefetchCount = 1;
                        ep.UseMessageRetry(r => r.Incremental(2, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(20)));
                        ep.ConfigureConsumer<GenerateIdentityConsumer>(rabbit);
                    });

                    cfg.ReceiveEndpoint($"{QueueNames.TrackingOrderRequestEventQueue}", ep =>
                    {
                        ep.PrefetchCount = 30;
                        ep.UseMessageRetry(r => r.Incremental(2, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(20)));
                        ep.ConfigureConsumer<TrackingOrderRequestEventConsumer>(rabbit);
                    });

                    cfg.ReceiveEndpoint($"{QueueNames.OrderRequestChangeStatusQueue}", ep =>
                    {
                        ep.PrefetchCount = 20;
                        ep.UseMessageRetry(r => r.Incremental(2, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(20)));
                        ep.ConfigureConsumer<OrderRequestChangeStatusConsumer>(rabbit);
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
