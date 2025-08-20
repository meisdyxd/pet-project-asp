namespace DirectoryService.Presentation.RegisterServices;

public static class RegisterSwagger
{
    public static IServiceCollection SwaggerConfigure(this IServiceCollection services)
    {
        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new()
            {
                Version = "v1",
                Description = "Тестовый сваггер",
                Title = "Pet Project",
            });
        });

        return services;
    }
}
