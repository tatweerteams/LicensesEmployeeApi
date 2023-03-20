using Infra;
using MassTransit;
using ReciveAPI.Consumer;
using SharedTatweerSendData.DTOs;

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
                provider.AddConsumer<ReciveOrderRequestConsumer>();

                provider.AddBus(rabbit => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(
                    mqOptions.RabbitMqAddress,
                    h =>
                    {
                        h.Username(mqOptions.RabbitMqUserName);
                        h.Password(mqOptions.RabbitMqPassword);
                    });

                    cfg.ReceiveEndpoint($"{QueueNames.ReciveOrderRequestQueue}", ep =>
                    {
                        ep.PrefetchCount = 1;
                        ep.UseMessageRetry(r => r.Incremental(2, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(20)));
                        ep.ConfigureConsumer<ReciveOrderRequestConsumer>(rabbit);
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
