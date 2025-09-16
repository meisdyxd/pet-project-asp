using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Contracts.Requests;

namespace DirectoryService.Application.CQRS.Commands.AddDepartment;

public class AddDepartmentCommand(AddDepartmentRequest request) : ICommand
{
    public AddDepartmentRequest Request { get; init; } = request;
}