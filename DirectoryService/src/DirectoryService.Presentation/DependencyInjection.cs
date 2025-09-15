using DirectoryService.Presentation.Middlewares;
using Serilog;

namespace DirectoryService.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Seq")
            ?? throw new Exception("Seq connection string not found");
        
        services.AddSerilog(o =>
        {
            o.ReadFrom.Configuration(configuration)
                .WriteTo.Seq(connectionString);
        });

        services.AddLogging();

        return services;
    }


    public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomExceptionMiddleware>();
        
        return app;
    }
}