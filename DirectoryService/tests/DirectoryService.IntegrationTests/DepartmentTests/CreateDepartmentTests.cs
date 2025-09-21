using DirectoryService.Application.CQRS.Commands.Departments.AddDepartment;
using DirectoryService.IntegrationTests.Infrastructure;
using DirectoryService.Domain.ValueObjects.Department;
using DirectoryService.Contracts.Errors;
using Microsoft.EntityFrameworkCore;
using CSharpFunctionalExtensions;

namespace DirectoryService.IntegrationTests.DepartmentTests;

public class CreateDepartmentTests : DirectoryServiceTestBase
{
    public CreateDepartmentTests(DirectoryServiceWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateDepartment_With_Valid_Data_Should_Succeed()
    {
        //arrange
        var locationId = await CreateLocation("Локация 1", "Первомая");
        var identifier = Identifier.Create("test").Value;
        var name = Name.Create("Тестовое подразделение").Value;

        //act
        var resultSut = await ExecuteHandler<AddDepartmentCommandHandler, UnitResult<ErrorList>>(async handler =>
        {
            var ct = new CancellationTokenSource().Token;
            var command = new AddDepartmentCommand(new()
            {
                Identifier = identifier.Value,
                Name = name.Value,
                LocationIds = [locationId],
                ParentId = null
            });

            return await handler.Handle(command, ct);
        });

        // assert
        Assert.True(resultSut.IsSuccess);

        var department = await ExecuteInDb(async context =>
        {
            return await context.Departments.FirstOrDefaultAsync(d => d.Identifier == identifier);
        });
        
        Assert.NotNull(department);
        Assert.Equal(identifier, department.Identifier);
        Assert.Equal(name, department.Name);
        Assert.True(department.Id != default);
    }

    [Fact]
    public async Task CreateDepartment_With_Invalid_Data_Should_Failed()
    {
        //arrange
        var locationId = await CreateLocation("Локация 1", "Первомая");
        var identifier = Identifier.Create("test").Value;
        var name = string.Empty; //invalid data

        //act
        var resultSut = await ExecuteHandler<AddDepartmentCommandHandler, UnitResult<ErrorList>>(async handler =>
        {
            var ct = new CancellationTokenSource().Token;
            var command = new AddDepartmentCommand(new()
            {
                Identifier = identifier.Value,
                Name = name,
                LocationIds = [locationId],
                ParentId = null
            });

            return await handler.Handle(command, ct);
        });

        // assert
        Assert.True(resultSut.IsFailure);
        Assert.Equal(400, resultSut.Error.StatusCode);
    }

    [Fact]
    public async Task CreateDepartment_With_Parent_Should_Succeed()
    {
        //arrange
        var locationId = await CreateLocation("Локация 1", "Первомая");
        var identifierFirst = Identifier.Create("first").Value;
        var nameFirst = "один";
        var identifierSecond = Identifier.Create("second").Value;
        var nameSecond = "два";
        var parentDepartment = await CreateDepartment(nameFirst, identifierFirst.Value, [locationId], null);

        //act
        var resultSutSecond = await ExecuteHandler<AddDepartmentCommandHandler, UnitResult<ErrorList>>(async handler =>
        {
            var ct = new CancellationTokenSource().Token;
            var command = new AddDepartmentCommand(new()
            {
                Identifier = identifierSecond.Value,
                Name = nameSecond,
                LocationIds = [locationId],
                ParentId = parentDepartment!.Id
            });

            return await handler.Handle(command, ct);
        });

        // assert
        Assert.True(resultSutSecond.IsSuccess);

        var department = await ExecuteInDb(async context =>
        {
            return await context.Departments.FirstOrDefaultAsync(d => d.Identifier == identifierSecond);
        });

        Assert.NotNull(department);
        Assert.Equal(identifierSecond, department.Identifier);
        Assert.Equal(Name.Create(nameSecond).Value, department.Name);
        Assert.True(department.Id != default);
        Assert.Equal(department.ParentId, parentDepartment!.Id);
    }

    [Fact]
    public async Task CreateDepartment_With_Exist_Identifier_Should_Failed()
    {
        //arrange
        var locationId = await CreateLocation("Локация 1", "Первомая");
        var identifierFirst = Identifier.Create("first").Value;
        var nameFirst = "один";
        var identifierSecond = Identifier.Create("first").Value;
        var nameSecond = "два";
        var parentDepartment = await CreateDepartment(nameFirst, identifierFirst.Value, [locationId], null);

        //act
        var resultSutSecond = await ExecuteHandler<AddDepartmentCommandHandler, UnitResult<ErrorList>>(async handler =>
        {
            var ct = new CancellationTokenSource().Token;
            var command = new AddDepartmentCommand(new()
            {
                Identifier = identifierSecond.Value,
                Name = nameSecond,
                LocationIds = [locationId],
                ParentId = parentDepartment!.Id
            });

            return await handler.Handle(command, ct);
        });

        // assert
        Assert.True(resultSutSecond.IsFailure);
    }
}
