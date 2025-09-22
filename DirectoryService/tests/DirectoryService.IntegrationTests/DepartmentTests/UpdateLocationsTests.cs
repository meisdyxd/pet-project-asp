using CSharpFunctionalExtensions;
using DirectoryService.Application.CQRS.Commands.Departments.UpdateLocations;
using DirectoryService.Contracts.Errors;
using DirectoryService.Domain.ValueObjects.Department;
using DirectoryService.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.IntegrationTests.DepartmentTests;

public class UpdateLocationsTests : DirectoryServiceTestBase
{
    public UpdateLocationsTests(DirectoryServiceWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task UpdateLocations_With_Valid_Data_Should_Succeed()
    {
        //arrange
        var locationId1 = await CreateLocation("Локация 1", "Первомая");
        var locationId2 = await CreateLocation("Локация 2", "Второмая");
        var locationId3 = await CreateLocation("Локация 3", "Третьемая");
        var locationId4 = await CreateLocation("Локация 4", "Четвертомая");
        var identifier = Identifier.Create("test").Value;
        var name = Name.Create("test").Value;
        var department = await CreateDepartment(name.Value, identifier.Value, [locationId1, locationId2], null);

        //act
        var resultSut = await ExecuteHandler<UpdateLocationsCommandHandler, UnitResult<ErrorList>>(async handler =>
        {
            var ct = new CancellationTokenSource().Token;
            var command = new UpdateLocationsCommand(department.Id, new()
            {
                LocationIds = [locationId3, locationId4]
            });
            return await handler.Handle(command, ct);
        });

        // assert
        Assert.True(resultSut.IsSuccess);

        var departmentAfter = await ExecuteInDb(async context =>
        {
            return await context.Departments
                .Include(d => d.DepartmentLocations)
                .FirstOrDefaultAsync(d => d.Identifier == identifier);
        });

        Assert.NotNull(departmentAfter);

        var departmentLocations = departmentAfter.DepartmentLocations.Select(dl => dl.LocationId);
        Assert.Contains(locationId3, departmentLocations);
        Assert.Contains(locationId4, departmentLocations);
    }

    [Fact]
    public async Task UpdateLocations_With_Invalid_Data_Should_Failed()
    {
        //arrange
        var locationId1 = await CreateLocation("Локация 1", "Первомая");
        var locationId2 = await CreateLocation("Локация 2", "Второмая");
        var identifier = Identifier.Create("test").Value;
        var name = Name.Create("test").Value;
        var department = await CreateDepartment(name.Value, identifier.Value, [locationId1, locationId2], null);

        //act
        var resultSut = await ExecuteHandler<UpdateLocationsCommandHandler, UnitResult<ErrorList>>(async handler =>
        {
            var ct = new CancellationTokenSource().Token;
            var command = new UpdateLocationsCommand(department.Id, new()
            {
                LocationIds = []
            });
            return await handler.Handle(command, ct);
        });

        // assert
        Assert.True(resultSut.IsFailure);
    }
}
