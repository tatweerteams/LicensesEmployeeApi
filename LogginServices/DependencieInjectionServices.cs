

using infra.rabbitMq.rabbitMqServices;
using Infra;
using Infra.Repository.RepositoryDapper;
using Infra.Services.rabbitMq;
using Infra.Utili;
using LogginDomain;
using LogginServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Repository.Repository;
using UnitOfWork.Work;

namespace TatweerLogginServices
{
    public static class DependencieInjectionServices
    {
        public static IServiceCollection AddDependencieInjection(this IServiceCollection services)
        {

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<LogginContextDB>));


            services.AddMemoryCache();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<EventHandlerUtili>();
            services.TryAddSingleton<HelperUtili>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(RepositoryDapper<>));

            services.AddScoped(typeof(ISendNotifyServices<>), typeof(SendNotifyServices<>));
            services.AddScoped<IInsertLogginDataServices, InsertLogginDataServices>();


            return services;
        }

        public static IServiceCollection AddConnactionDb(this IServiceCollection services)
        {
            var CustomerBaseConnectionString = string.Empty;

            using (var scope = services.BuildServiceProvider()) CustomerBaseConnectionString = scope
                    .GetService<IConfiguration>()
                    .GetConnectionString($"{nameof(LogginContextDB)}");

            if (string.IsNullOrEmpty(CustomerBaseConnectionString)) throw new Exception($"connection string for {nameof(LogginContextDB)} is missing from the appsettings.json file!!");

            services.AddDbContextPool<LogginContextDB>(opt => opt.UseSqlServer(CustomerBaseConnectionString, x => x.MigrationsHistoryTable("__MigrationsHistoryForCustomerBaseDb", "migrations"))
               .EnableDetailedErrors()
               .EnableSensitiveDataLogging());

            using (var scope = services.BuildServiceProvider())
            {
                var customerDb = scope.GetService<LogginContextDB>();
                customerDb?.Database?.Migrate();
                //customerDb.SeedRegionBranchs().Wait();
            }

            return services;
        }
    }
}
