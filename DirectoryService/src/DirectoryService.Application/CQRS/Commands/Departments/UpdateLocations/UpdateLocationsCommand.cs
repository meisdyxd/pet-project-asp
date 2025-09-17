using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Contracts.Requests.DepartmentRequests;

namespace DirectoryService.Application.CQRS.Commands.Departments.UpdateLocations;

public sealed record UpdateLocationsCommand(Guid departmentId, UpdateLocationsRequest request) : ICommand
{
    public Guid DepartmentId { get; set; } = departmentId;
    public UpdateLocationsRequest Request { get; set; } = request;
}