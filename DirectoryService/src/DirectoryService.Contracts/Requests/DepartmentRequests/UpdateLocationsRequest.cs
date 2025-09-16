namespace DirectoryService.Contracts.Requests.DepartmentRequests;

public class UpdateLocationsRequest
{
    public IEnumerable<Guid> LocationIds { get; set; } = [];
}