using DirectoryService.Infrastructure.Database.Context;
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
        services.AddDbContext(configuration);

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

    private static ILoggerFactory GetLoggerFactory() 
        => LoggerFactory.Create(o =>
        {
            o.AddConsole();
            o.SetMinimumLevel(LogLevel.Warning);
        });
}
