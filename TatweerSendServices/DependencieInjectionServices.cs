

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
using SendEventBus.PublishEvents;
using TatweerSendDomain;
using TatweerSendServices.services;
using TatweerSendServices.servicesValidation;
using UnitOfWork.Work;

namespace TatweerSendServices
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

            services.AddScoped<IRegionServices, RegionServices>();
            services.AddScoped<IRegionValidationServices, RegionValidationServices>();

            services.AddScoped<IBankServices, BankServices>();
            services.AddScoped<IBankValidationServices, BankValidationServices>();

            services.AddScoped<IBankRegionValidationServices, BankRegionValidationServices>();

            services.AddScoped<IBankRegionServices, BankRegionServices>();
            services.AddScoped<IBankRegionValidationServices, BankRegionValidationServices>();


            services.AddScoped<IBranchServices, BranchServices>();
            services.AddScoped<IBranchValidationServices, BranchValidationServices>();

            services.AddScoped<IOrderRequestServices, OrderRequestServices>();
            services.AddScoped<IOrderRequestValidationServices, OrderRequestValidationServices>();

            services.AddScoped<IOrderRequestItemServices, OrderRequestItemServices>();
            services.AddScoped<IOrderItemValidationServices, OrderItemValidationServices>();


            services.AddScoped<IReasonRefuseServices, ReasonRefuseServices>();
            services.AddScoped<IReasonRefuseValidationServices, ReasonRefuseValidationServices>();

            services.AddScoped<IAccountServices, AccountServices>();
            services.AddScoped<IAccountValidationServices, AccountValidationServices>();

            services.AddScoped<IReportServices, ReportServices>();


            services.AddScoped<TrackingOrderEventPublish>();
            services.AddScoped<LogginDataPublish>();

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
            });

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
