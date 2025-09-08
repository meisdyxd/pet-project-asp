using DirectoryService.Presentation.Middlewares;

namespace DirectoryService.Presentation;

public static class DependencyInjection
{
    public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomExceptionMiddleware>();
        
        return app;
    }
}