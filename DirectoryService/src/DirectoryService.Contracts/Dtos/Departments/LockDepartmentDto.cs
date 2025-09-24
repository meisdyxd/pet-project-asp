namespace DirectoryService.Contracts.Dtos.Departments;

public sealed record LockDepartmentDto
{
    public LockDepartmentDto(
        Guid id,
        string path,
        bool isActive,
        Guid parentId)
    {
        Id = id;
        Path = path;
        IsActive = isActive;
        ParentId = parentId;
    }
    
    public Guid Id { get; init; }
    public string Path { get; init; }
    public bool IsActive { get; init; }
    public Guid ParentId { get; init; }
}