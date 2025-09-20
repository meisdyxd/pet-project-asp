namespace DirectoryService.Contracts.Requests.DepartmentRequests;

public sealed record ChangeParentRequest(Guid? ParentId)
{
    public Guid? ParentId { get; init; } = ParentId;    
}
