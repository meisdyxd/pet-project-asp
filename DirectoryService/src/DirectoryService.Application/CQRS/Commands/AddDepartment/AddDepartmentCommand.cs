using DirectoryService.Application.Interfaces.CQRS;

namespace DirectoryService.Application.CQRS.Commands.AddDepartment;

public class AddDepartmentCommand : ICommand
{
    public string Name { get; init; }
    public string Identifier { get; init; }
    public Guid? ParentId { get; init; }
    public IEnumerable<Guid> LocationIds { get; init; }

    public AddDepartmentCommand(
        string name,
        string identifier,
        Guid? parentId,
        IEnumerable<Guid> locationIds)
    {
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        LocationIds = locationIds;
    }
}