using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Contracts.Requests.DepartmentRequests;

namespace DirectoryService.Application.CQRS.Commands.Departments.AddDepartment;

public class AddDepartmentCommand(AddDepartmentRequest request) : ICommand
{
    public AddDepartmentRequest Request { get; init; } = request;
}