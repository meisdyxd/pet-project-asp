namespace DirectoryService.Contracts.Requests.PositionRequests;

public sealed record AddPositionRequest
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public IEnumerable<Guid> DepartmentIds { get; init; } = [];
}