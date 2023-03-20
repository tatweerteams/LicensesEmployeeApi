using Hangfire;
using infra.rabbitMq.rabbitMqServices;
using Infra;
using Infra.Repository;
using Infra.Services.rabbitMq;
using Infra.Utili;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Repository.Repository;
using SendEventBus.PublishEvents;
using Services;
using TrackingServices.Domain;
using UnitOfWork.Work;


namespace IdentityServices
{
    public static class DependencieInjectionServices
    {
        public static IServiceCollection AddTrackingServiceDI(this IServiceCollection services)
        {

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<ReciveDbContext>));
            services.AddScoped(typeof(IRepositoryWriteOnly<>), typeof(RepositoryWriteOnly<>));
            services.AddScoped(typeof(IRepositoryReadOnly<>), typeof(RepositoryReadOnly<>));

            services.AddMemoryCache();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<EventHandlerUtili>();
            services.AddScoped<OrderRequestChengeStauesPublish>();

            services.AddScoped(typeof(ISendNotifyServices<>), typeof(SendNotifyServices<>));



            services.AddScoped<ITrackServices, TrackServices>();
            return services;
        }

        public static IServiceCollection AddTrackingServiceDb(this IServiceCollection services)
        {
            var CustomerBaseConnectionString = string.Empty;

            using (var scope = services.BuildServiceProvider()) CustomerBaseConnectionString = scope
                    .GetService<IConfiguration>()
                    .GetConnectionString($"{nameof(ReciveDbContext)}");

            if (string.IsNullOrEmpty(CustomerBaseConnectionString)) throw new Exception($"connection string for {nameof(ReciveDbContext)} is missing from the appsettings.json file!!");

            services.AddDbContextPool<ReciveDbContext>(opt => opt.UseSqlServer(CustomerBaseConnectionString, x => x.MigrationsHistoryTable("__MigrationsHistoryForCustomerBaseDb", "migrations"))
               .EnableDetailedErrors()
               .EnableSensitiveDataLogging());

            using (var scope = services.BuildServiceProvider())
            {
                var customerDb = scope.GetService<ReciveDbContext>();
                customerDb?.Database?.Migrate();
                //customerDb.SeedRegionBranchs().Wait();
            }

            return services;
        }

        public static IApplicationBuilder UseHangFire(this IApplicationBuilder app)
        {


            RecurringJob.AddOrUpdate<ITrackServices>(
                "Run Background service Change Order Request Is Printing",
                x => x.GetOrderRequestIsPrinting(),
                "*/1 * * * *"
                );

            RecurringJob.AddOrUpdate<ITrackServices>(
              "Run Background service Change Order Request Is Printed",
              x => x.GetOrderRequestIsDone(),
               "*/1 * * * *"
              );
            return app;
        }


    }
}
