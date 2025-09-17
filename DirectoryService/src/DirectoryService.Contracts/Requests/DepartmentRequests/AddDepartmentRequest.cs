namespace DirectoryService.Contracts.Requests.DepartmentRequests;

public sealed record AddDepartmentRequest
{
    public string Name { get; init; } = string.Empty;
    public string Identifier { get; init; } = string.Empty;
    public Guid? ParentId { get; init; }
    public IEnumerable<Guid> LocationIds { get; init; } = [];
}