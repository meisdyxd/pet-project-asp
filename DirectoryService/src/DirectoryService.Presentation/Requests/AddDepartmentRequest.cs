using DirectoryService.Application.CQRS.Commands.AddDepartment;

namespace DirectoryService.Presentation.Requests;

public class AddDepartmentRequest
{
    public string Name { get; init; } = null!;
    public string Identifier { get; init; } = null!;
    public Guid? ParentId { get; init; }
    public IEnumerable<Guid> LocationIds { get; init; } = [];
    
    public AddDepartmentCommand ToCommand()
        => new(Name, Identifier, ParentId, LocationIds);
}