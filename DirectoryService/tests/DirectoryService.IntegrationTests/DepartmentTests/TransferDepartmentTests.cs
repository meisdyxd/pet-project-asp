using CSharpFunctionalExtensions;
using DirectoryService.Application.CQRS.Commands.Departments.TransferDepartment;
using DirectoryService.Contracts.Errors;
using DirectoryService.Domain.ValueObjects.Department;
using DirectoryService.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.IntegrationTests.DepartmentTests;

public class TransferDepartmentTests : DirectoryServiceTestBase
{
    public TransferDepartmentTests(DirectoryServiceWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task TransferDepartment_With_Valid_Data_Should_Succeed()
    {
        //arrange
        var locationId1 = await CreateLocation("Локация 1", "Первомая");

        var identifier1 = Identifier.Create("main").Value;
        var name1 = Name.Create("Главный").Value;
        var department1 = await CreateDepartment(name1.Value, identifier1.Value, [locationId1], null);

        var identifier2 = Identifier.Create("it").Value;
        var name2 = Name.Create("Айти").Value;
        var department2 = await CreateDepartment(name2.Value, identifier2.Value, [locationId1], department1.Id);

        var identifier3 = Identifier.Create("backend").Value;
        var name3 = Name.Create("бекенд").Value;
        var department3 = await CreateDepartment(name3.Value, identifier3.Value, [locationId1], department2.Id);

        var identifier4 = Identifier.Create("frontend").Value;
        var name4 = Name.Create("фронтедн").Value;
        var department4 = await CreateDepartment(name4.Value, identifier4.Value, [locationId1], department2.Id);

        var identifier5 = Identifier.Create("dev").Value;
        var name5 = Name.Create("дев").Value;
        var department5 = await CreateDepartment(name5.Value, identifier5.Value, [locationId1], department3.Id);

        var identifier6 = Identifier.Create("qa").Value;
        var name6 = Name.Create("тестеры").Value;
        var department6 = await CreateDepartment(name6.Value, identifier6.Value, [locationId1], department3.Id);

        //act
        var resultSut = await ExecuteHandler<TransferDepartmentCommandHandler, UnitResult<ErrorList>>(async handler =>
        {
            var ct = new CancellationTokenSource().Token;
            var command = new TransferDepartmentCommand(department3.Id, new(department1.Id));
            return await handler.Handle(command, ct);
        });

        // assert
        Assert.True(resultSut.IsSuccess);

        var departmentSourceAfter = await ExecuteInDb(async context =>
        {
            return await context.Departments.FirstOrDefaultAsync(d => d.Identifier == identifier3);
        });

        var departmentChildAfter = await ExecuteInDb(async context =>
        {
            return await context.Departments.FirstOrDefaultAsync(d => d.Identifier == identifier5);
        });

        Assert.Equal(departmentSourceAfter!.ParentId, department1.Id);
        Assert.DoesNotContain(identifier2.Value, departmentSourceAfter.Path.Value);
        Assert.DoesNotContain(identifier2.Value, departmentChildAfter!.Path.Value);
    }

    [Fact]
    public async Task TransferDepartment_To_Child_Department_Should_Failed()
    {
        //arrange
        var locationId1 = await CreateLocation("Локация 1", "Первомая");

        var identifier1 = Identifier.Create("main").Value;
        var name1 = Name.Create("Главный").Value;
        var department1 = await CreateDepartment(name1.Value, identifier1.Value, [locationId1], null);

        var identifier2 = Identifier.Create("it").Value;
        var name2 = Name.Create("Айти").Value;
        var department2 = await CreateDepartment(name2.Value, identifier2.Value, [locationId1], department1.Id);

        var identifier3 = Identifier.Create("backend").Value;
        var name3 = Name.Create("бекенд").Value;
        var department3 = await CreateDepartment(name3.Value, identifier3.Value, [locationId1], department2.Id);

        var identifier4 = Identifier.Create("frontend").Value;
        var name4 = Name.Create("фронтедн").Value;
        var department4 = await CreateDepartment(name4.Value, identifier4.Value, [locationId1], department2.Id);

        var identifier5 = Identifier.Create("dev").Value;
        var name5 = Name.Create("дев").Value;
        var department5 = await CreateDepartment(name5.Value, identifier5.Value, [locationId1], department3.Id);

        var identifier6 = Identifier.Create("qa").Value;
        var name6 = Name.Create("тестеры").Value;
        var department6 = await CreateDepartment(name6.Value, identifier6.Value, [locationId1], department3.Id);

        //act
        var resultSut = await ExecuteHandler<TransferDepartmentCommandHandler, UnitResult<ErrorList>>(async handler =>
        {
            var ct = new CancellationTokenSource().Token;
            var command = new TransferDepartmentCommand(department1.Id, new(department3.Id));
            return await handler.Handle(command, ct);
        });

        // assert
        Assert.True(resultSut.IsFailure);
    }
}
