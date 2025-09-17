namespace DirectoryService.Contracts.Requests.DepartmentRequests;

public sealed record UpdateLocationsRequest
{
    public IEnumerable<Guid> LocationIds { get; set; } = [];
}