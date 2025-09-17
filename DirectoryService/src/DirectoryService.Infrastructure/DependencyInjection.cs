using DirectoryService.Application.Interfaces.Database.IRepositories;
using DirectoryService.Application.Interfaces.Database;
using DirectoryService.Infrastructure.Database.Context;
using DirectoryService.Infrastructure.Repositories;
using DirectoryService.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
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
            .AddRepositories()
            .AddTransactionExtensions();

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

        services.AddScoped<IReadDbContext, DirectoryServiceContext>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ILocationsRepository, LocationsRepository>();
        services.AddScoped<IDepartmentsRepository, DepartmentsRepository>();
        services.AddScoped<IPositionsRepository, PositionsRepository>();
        
        return services;
    }

    private static IServiceCollection AddTransactionExtensions(this IServiceCollection services)
    {
        services.AddScoped<ITransactionManager, TransactionManager>();
        
        return services;
    }

    private static ILoggerFactory GetLoggerFactory() 
        => LoggerFactory.Create(o =>
        {
            o.AddConsole();
            o.SetMinimumLevel(LogLevel.Warning);
        });
}
