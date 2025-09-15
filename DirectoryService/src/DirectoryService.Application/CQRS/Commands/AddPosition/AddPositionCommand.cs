using DirectoryService.Application.Interfaces.CQRS;

namespace DirectoryService.Application.CQRS.Commands.AddPosition;

public class AddPositionCommand : ICommand
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public IEnumerable<Guid> DepartmentIds { get; init; }

    public AddPositionCommand(
        string name,
        string? description,
        IEnumerable<Guid> departmentIds)
    {
        Name = name;
        Description = description;
        DepartmentIds = departmentIds;
    }
}