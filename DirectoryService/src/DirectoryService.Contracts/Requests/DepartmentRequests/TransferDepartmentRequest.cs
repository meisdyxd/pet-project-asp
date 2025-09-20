namespace DirectoryService.Contracts.Requests.DepartmentRequests;

public sealed record TransferDepartmentRequest(Guid? ParentId)
{
    public Guid? ParentId { get; init; } = ParentId;    
}
