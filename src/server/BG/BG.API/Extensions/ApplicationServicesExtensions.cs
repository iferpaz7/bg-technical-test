using System.Reflection;
using System.Threading.RateLimiting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Scrutor;

namespace BG.API.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        //MAPPING DTOs
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddControllers().AddNewtonsoftJson(x =>
        {
            x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            x.SerializerSettings.ContractResolver = new DefaultContractResolver
                { NamingStrategy = new CamelCaseNamingStrategy() };
        });
        ;

        //Restrict request
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    httpContext.Connection.RemoteIpAddress?.ToString(), partition =>
                        new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 10,
                            QueueLimit = 0,
                            Window = TimeSpan.FromSeconds(1)
                        }));
        });

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();

        //DYNAMIC DEPENDENCY INJECTION WITH SCRUTOR
        string[] nameSpaces =
        [
            "BG.Application.Services",
            "Common.Utils.Security.Services",
            "BG.Infrastructure.Repositories.Implementations"
        ];
        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(classes => classes.InNamespaces(nameSpaces))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithTransientLifetime()
        );

        return services;
    }
}