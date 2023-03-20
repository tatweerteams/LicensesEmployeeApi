using infra.rabbitMq.rabbitMqServices;
using Infra;
using Infra.Repository;
using Infra.Services.rabbitMq;
using Infra.Utili;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ReciveAPI.Domain;
using Repository.Repository;
using UnitOfWork.Work;

namespace IdentityServices
{
    public static class DependencieInjectionServices
    {
        public static IServiceCollection AddDependencieInjection(this IServiceCollection services)
        {


            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<TempDatabase>));
            services.AddScoped(typeof(IRepositoryWriteOnly<>), typeof(RepositoryWriteOnly<>));
            services.AddScoped(typeof(IRepositoryReadOnly<>), typeof(RepositoryReadOnly<>));

            services.AddMemoryCache();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<EventHandlerUtili>();
            services.TryAddSingleton<HelperUtili>();

            services.AddScoped(typeof(ISendNotifyServices<>), typeof(SendNotifyServices<>));


            //services.AddScoped<IAuthUserServices, AuthUserServices>();

            //services.AddScoped<IPermisstionServices, PermisstionServices>();
            //services.AddScoped<IPermisstionValidationServices, PermisstionValidationServices>();


            //services.AddScoped<IRoleValidationServices, RoleValidationServices>();
            //services.AddScoped<IRoleServices, RoleServices>();

            //services.AddScoped<IUserValidationServices, UserValidationServices>();
            //services.AddScoped<IUserServices, UserServices>();


            //services.AddScoped<IUserAuth, UserAuth>();
            //services.AddScoped<IUserAuthValidationServices, UserAuthValidationServices>();

            return services;

        }

        public static IServiceCollection AddConnactionDb(this IServiceCollection services)
        {
            var CustomerBaseConnectionString = string.Empty;

            using (var scope = services.BuildServiceProvider()) CustomerBaseConnectionString = scope
                    .GetService<IConfiguration>()
                    .GetConnectionString($"{nameof(TempDatabase)}");

            if (string.IsNullOrEmpty(CustomerBaseConnectionString)) throw new Exception($"connection string for {nameof(TempDatabase)} is missing from the appsettings.json file!!");

            services.AddDbContextPool<TempDatabase>(opt => opt.UseSqlServer(CustomerBaseConnectionString, x => x.MigrationsHistoryTable("__MigrationsHistoryForCustomerBaseDb", "migrations"))
               .EnableDetailedErrors()
               .EnableSensitiveDataLogging());

            using (var scope = services.BuildServiceProvider())
            {
                var customerDb = scope.GetService<TempDatabase>();
                customerDb?.Database?.Migrate();
                //customerDb.SeedRegionBranchs().Wait();
            }

            return services;
        }


    }
}
