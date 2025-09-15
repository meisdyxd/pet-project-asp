using DirectoryService.Application.Interfaces.IRepositories;
using DirectoryService.Infrastructure.Database.Context;
using DirectoryService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services
            .AddDbContext(configuration)
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = configuration.GetConnectionString("Database");

        services.AddDbContext<DirectoryServiceContext>(options =>
        {
            options.UseNpgsql(config);
            options.UseLoggerFactory(GetLoggerFactory());
        });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ILocationsRepository, LocationsRepository>();
        services.AddScoped<IDepartmentsRepository, DepartmentsRepository>();
        
        return services;
    }

    private static ILoggerFactory GetLoggerFactory() 
        => LoggerFactory.Create(o =>
        {
            o.AddConsole();
            o.SetMinimumLevel(LogLevel.Warning);
        });
}
