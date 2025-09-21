using CSharpFunctionalExtensions;
using DirectoryService.Application.CQRS.Commands.Departments.AddDepartment;
using DirectoryService.Contracts.Errors;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects.Common;
using DirectoryService.Domain.ValueObjects.Department;
using DirectoryService.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DirectoryService.IntegrationTests.Infrastructure;

public class DirectoryServiceTestBase : IClassFixture<DirectoryServiceWebFactory>, IAsyncLifetime
{
    protected readonly IServiceProvider _services;
    protected readonly Func<Task> _resetDatabase;

    public DirectoryServiceTestBase(DirectoryServiceWebFactory factory)
    {
        _services = factory.Services;
        _resetDatabase = factory.ResetDatabase;
    }

    protected async Task<Guid> CreateLocation(string locationName, string street)
    {
        await using (var scopeLocation = _services.CreateAsyncScope())
        {
            var dbContext = scopeLocation.ServiceProvider.GetRequiredService<DirectoryServiceContext>();

            var ct = new CancellationTokenSource().Token;
            var location = GetLocation(locationName, street);
            await dbContext.Locations.AddAsync(location, ct);
            await dbContext.SaveChangesAsync(ct);
            return location.Id;
        }
    }

    protected async Task<Department> CreateDepartment(string name, string identifier, Guid[] locationIds, Guid? parentId)
    {
        var resultSutFirst = await ExecuteHandler<AddDepartmentCommandHandler, UnitResult<ErrorList>>(async handler =>
        {
            var ct = new CancellationTokenSource().Token;
            var command = new AddDepartmentCommand(new()
            {
                Identifier = Identifier.Create(identifier).Value.Value,
                Name = Name.Create(name).Value.Value,
                LocationIds = locationIds,
                ParentId = parentId
            });

            return await handler.Handle(command, ct);
        });

        return await ExecuteInDb(async context =>
        {
            return await context.Departments
                .Include(d => d.DepartmentLocations)
                .FirstAsync(d => d.Identifier == Identifier.Create(identifier).Value);
        });
    }

    protected async Task<T> ExecuteInDb<T>(Func<DirectoryServiceContext, Task<T>> func)
    {
        using var scope = _services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DirectoryServiceContext>();
        return await func(dbContext);
    }

    protected async Task<TResult> ExecuteHandler<THandler, TResult>(Func<THandler, Task<TResult>> func)
        where THandler : notnull
    {
        using var scope = _services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<THandler>();
        return await func(handler);
    }

    private Location GetLocation(string locationName, string street)
    {
        var name = Domain.ValueObjects.Location.Name.Create(locationName).Value;
        var address = Address.Create("Россия", "Чувашская республика", "Чебоксары", street, "8").Value;
        var timezone = IANATimezone.Create("Europe/Moscow").Value;

        return Location.Create(name, address, timezone).Value;
    }

    public async Task DisposeAsync() => await _resetDatabase();
    public Task InitializeAsync() => Task.CompletedTask;
}
