

using GenerateIdentityServices.Services;
using infra.rabbitMq.rabbitMqServices;
using Infra;
using Infra.Repository;
using Infra.Repository.RepositoryDapper;
using Infra.Services.rabbitMq;
using Infra.Utili;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Repository.Repository;
using TatweerSendDomain;
using UnitOfWork.Work;

namespace GenerateIdentityServices
{
    public static class DependencieInjectionServices
    {
        public static IServiceCollection AddDependencieInjection(this IServiceCollection services)
        {

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<TatweerSendDbContext>));
            services.AddScoped(typeof(IRepositoryWriteOnly<>), typeof(RepositoryWriteOnly<>));
            services.AddScoped(typeof(IRepositoryReadOnly<>), typeof(RepositoryReadOnly<>));

            services.AddMemoryCache();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<EventHandlerUtili>();
            services.TryAddSingleton<HelperUtili>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(RepositoryDapper<>));

            services.AddScoped(typeof(ISendNotifyServices<>), typeof(SendNotifyServices<>));

            services.AddScoped<IGenerateIdentityRequestServices, GenerateIdentityRequestServices>();
            services.AddScoped<ITrackingOrderRequestEventServices, TrackingOrderRequestEventServices>();
            services.AddScoped<IChangeOrderRequestStatueServices, ChangeOrderRequestStatueServices>();

            return services;
        }

        public static IServiceCollection AddConnactionDb(this IServiceCollection services)
        {
            var CustomerBaseConnectionString = string.Empty;

            using (var scope = services.BuildServiceProvider()) CustomerBaseConnectionString = scope
                    .GetService<IConfiguration>()
                    .GetConnectionString($"{nameof(TatweerSendDbContext)}");

            if (string.IsNullOrEmpty(CustomerBaseConnectionString)) throw new Exception($"connection string for {nameof(TatweerSendDbContext)} is missing from the appsettings.json file!!");

            services.AddDbContextPool<TatweerSendDbContext>(opt => opt.UseSqlServer(CustomerBaseConnectionString, x => x.MigrationsHistoryTable("__MigrationsHistoryForCustomerBaseDb", "migrations"))
               .EnableDetailedErrors()
               .EnableSensitiveDataLogging());

            using (var scope = services.BuildServiceProvider())
            {
                var customerDb = scope.GetService<TatweerSendDbContext>();
                customerDb?.Database?.Migrate();
                //customerDb.SeedRegionBranchs().Wait();
            }

            return services;
        }





    }
}
