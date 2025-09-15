using System.Net;
using DirectoryService.Contracts;
using DirectoryService.Domain;

namespace DirectoryService.Presentation.Middlewares;

public class CustomExceptionMiddleware : IMiddleware
{
    private readonly ILogger<CustomExceptionMiddleware> _logger;

    public CustomExceptionMiddleware(ILogger<CustomExceptionMiddleware> logger)
    {
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(Envelope.Failure([Errors.Http.InternalServerError()]));
        }
    }
}