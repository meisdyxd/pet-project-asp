using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Contracts.Requests;
using DirectoryService.Contracts.Requests.DepartmentRequests;

namespace DirectoryService.Application.CQRS.Commands.Departments.UpdateLocations;

public class UpdateLocationsCommand(Guid departmentId, UpdateLocationsRequest request) : ICommand
{
    public Guid DepartmentId { get; set; } = departmentId;
    public UpdateLocationsRequest Request { get; set; } = request;
}