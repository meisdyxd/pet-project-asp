using DirectoryService.Application.CQRS.Commands.AddPosition;

namespace DirectoryService.Presentation.Requests;

public class AddPositionRequest
{
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public IEnumerable<Guid> DepartmentIds { get; init; } = [];
    
    public AddPositionCommand ToCommand()
        => new(Name, Description, DepartmentIds);
}