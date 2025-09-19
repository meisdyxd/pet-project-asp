namespace DirectoryService.Contracts.Requests;

public class AddPositionRequest
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public IEnumerable<Guid> DepartmentIds { get; init; } = [];
}