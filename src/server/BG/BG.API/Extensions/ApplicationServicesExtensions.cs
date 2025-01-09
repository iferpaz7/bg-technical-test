using BG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Scrutor;
using System.Reflection;

namespace BG.API.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        //MAPPING DTOs
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddDbContextPool<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly("BG.Infrastructure"))); //Migrations in infrastructure library

        services.AddControllers();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();

        //DYNAMIC DEPENDENCY INJECTION WITH SCRUTOR
        string[] nameSpaces =
            ["BG.Application.Interfaces.Services"];
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