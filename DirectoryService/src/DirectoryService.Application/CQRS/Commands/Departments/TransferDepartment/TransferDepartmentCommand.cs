using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Contracts.Requests.DepartmentRequests;

namespace DirectoryService.Application.CQRS.Commands.Departments.TransferDepartment;

public sealed record TransferDepartmentCommand(Guid DepartmentId, TransferDepartmentRequest Request) : ICommand
{
    public Guid DepartmentId { get; init; } = DepartmentId;
    public TransferDepartmentRequest Request { get; init; } = Request;
}