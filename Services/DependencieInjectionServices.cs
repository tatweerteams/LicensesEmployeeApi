using CollactionData.DTOs;
using Domain;
using IdentityServices.services;
using IdentityServices.ValidationServicess;
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
using UnitOfWork.Work;

namespace IdentityServices
{
    public static class DependencieInjectionServices
    {
        public static IServiceCollection AddDependencieInjection(this IServiceCollection services)
        {


            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<IdentityTatweerSendDB>));
            services.AddScoped(typeof(IRepositoryWriteOnly<>), typeof(RepositoryWriteOnly<>));

            services.AddMemoryCache();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<EventHandlerUtili>();
            services.TryAddSingleton<HelperUtili>();

            //services.AddScoped<IUnitOfWorkDapper, UnitOfWorkDapper>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(RepositoryDapper<>));

            services.AddScoped(typeof(ISendNotifyServices<>), typeof(SendNotifyServices<>));

            services.AddScoped<IAuthUserServices, AuthUserServices>();

            services.AddScoped<IPermisstionServices, PermisstionServices>();
            services.AddScoped<IPermisstionValidationServices, PermisstionValidationServices>();


            services.AddScoped<IRoleValidationServices, RoleValidationServices>();
            services.AddScoped<IRoleServices, RoleServices>();

            services.AddScoped<IUserValidationServices, UserValidationServices>();
            services.AddScoped<IUserServices, UserServices>();

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
            });

            //services.AddScoped<IUserAuth, UserAuth>();
            //services.AddScoped<IUserAuthValidationServices, UserAuthValidationServices>();

            return services;

        }

        public static IServiceCollection AddConnactionDb(this IServiceCollection services)
        {
            var CustomerBaseConnectionString = string.Empty;

            using (var scope = services.BuildServiceProvider()) CustomerBaseConnectionString = scope
                    .GetService<IConfiguration>()
                    .GetConnectionString($"{nameof(IdentityTatweerSendDB)}");

            if (string.IsNullOrEmpty(CustomerBaseConnectionString)) throw new Exception($"connection string for {nameof(IdentityTatweerSendDB)} is missing from the appsettings.json file!!");

            services.AddDbContextPool<IdentityTatweerSendDB>(opt => opt.UseSqlServer(CustomerBaseConnectionString, x => x.MigrationsHistoryTable("__MigrationsHistoryForCustomerBaseDb", "migrations"))
               .EnableDetailedErrors()
               .EnableSensitiveDataLogging());

            using (var scope = services.BuildServiceProvider())
            {
                var customerDb = scope.GetService<IdentityTatweerSendDB>();
                customerDb?.Database?.Migrate();
                //customerDb.SeedRegionBranchs().Wait();
            }

            return services;
        }

        public static IServiceCollection AddSectionFromAppSetting(this IServiceCollection services)
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                services.Configure<UserSystemDTO>(configuration.GetSection("UserSystem"));
            }
            return services;
        }
    }
}
