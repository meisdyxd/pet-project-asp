using DirectoryService.Application.Interfaces.CQRS;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddCQRS()
            .AddValidators();

        return services;
    }
    
    private static IServiceCollection AddCQRS(this IServiceCollection services)
    {
        services.Scan(scan =>
        {
            scan.FromAssemblies(typeof(DependencyInjection).Assembly)
                .AddClasses(classes => classes
                    .AssignableToAny(typeof(ICommandHandler<>), typeof(ICommandHandler<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime();
            
            scan.FromAssemblies(typeof(DependencyInjection).Assembly)
                .AddClasses(classes => classes
                    .AssignableTo(typeof(IQueryHandler<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime();
        });
        
        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        return services;
    }
}