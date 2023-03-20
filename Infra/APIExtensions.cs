using FilterAttributeWebAPI.Common;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.MemoryStorage;
using Infra.Utili;
using Infra.Utili.ConfigrationModels;
using Infra.Utili.Filters;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace Infra
{
    public static class APIExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, string apiName)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = apiName, Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            });
            return services;
        }

        //generic  ConfigureCors(this IServiceCollection services, string sectionName)
        private static string[] ConfigureCors(this IServiceCollection services)
        {
            List<string> options;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();

                var config = configuration?.GetSection("SpecificOrigins");

                services.Configure<List<string>>(config);
                options = configuration.GetOptions<List<string>>("SpecificOrigins");
            }

            return options.ToArray();
        }


        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            string[] origins = services.ConfigureCors();
            return services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                .WithOrigins(origins)
                .AllowAnyMethod()
                .AllowCredentials()
                .AllowAnyHeader()));
        }

        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(section).Bind(model);
            return model;
        }

        public static IServiceCollection AddCustomMvc(this IServiceCollection services, Assembly asmForMapperAndValidators)
        {
            services.AddHttpContextAccessor();
            //services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(10));

            services.AddHealthChecks();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers(config =>
            {
                config.Filters.Add<ExceptionHandlerUtili>();
                config.Filters.Add<ParametorModelFilter>();
            }).AddFluentValidation(options =>
            {
                options.ImplicitlyValidateChildProperties = true;
                options.ImplicitlyValidateRootCollectionElements = true;
                options.RegisterValidatorsFromAssembly(asmForMapperAndValidators);
                options.AutomaticValidationEnabled = true;
            });

            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer().
                UseMemoryStorage();
            });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
            });

            services.AddScoped<AntiforgeryMiddleware>();

            services.AddHangfireServer();

            //services.AddMediatR(asmForMapperAndValidators);
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            services.AddOptions();
            services.AddMvcCore();



            AddCorsPolicy(services);


            return services;


        }


        public static IServiceCollection AddJWT(this IServiceCollection services)
        {

            var TATWEER_API_SEND_OUTH = "OAuth";
            AppSettingsConfig appSettings = null;

            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                services.Configure<AppSettingsConfig>(configuration.GetSection("AppSettingsConfig"));
                appSettings = configuration.GetOptions<AppSettingsConfig>("AppSettingsConfig");
            }
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.Secret));

            //OAuth
            services.AddAuthentication(TATWEER_API_SEND_OUTH).AddJwtBearer(TATWEER_API_SEND_OUTH, config =>
            {
                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = HelperUtili.LifetimeValidator,
                    IssuerSigningKey = securityKey,
                    SaveSigninToken = true,
                };

                config.EventsType = typeof(EventHandlerUtili);
                config.SaveToken = true;
            });
            return services;
        }


        public static IApplicationBuilder UseCustomMvc(this IApplicationBuilder app, Action<IApplicationBuilder> funcApp = default)
        {


            funcApp?.Invoke(app);
            app.UseHealthChecks("/health");

            app.UseMiddleware<AntiforgeryMiddleware>();
            app.UseMiddleware<OperationCanceledMiddleware>();
            app.UseCorsPolicy();
            app.UseRouting();
            app.UseAuthentication().UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });


            return app;
        }

        public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app) => app.UseCors("CorsPolicy");


    }
}
