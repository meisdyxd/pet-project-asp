using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Contracts.Requests.DepartmentRequests;

namespace DirectoryService.Application.CQRS.Commands.Departments.ChangeParent;

public sealed record ChangeParentCommand(Guid DepartmentId, ChangeParentRequest Request) : ICommand
{
    public Guid DepartmentId { get; init; } = DepartmentId;
    public ChangeParentRequest Request { get; init; } = Request;
}