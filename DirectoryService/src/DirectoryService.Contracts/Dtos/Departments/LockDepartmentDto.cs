namespace DirectoryService.Contracts.Dtos.Departments;

public sealed record LockDepartmentDto(Guid id, string path, bool is_active, Guid parent_id)
{
    public Guid Id { get; init; } = id;
    public string Path { get; init; } = path;
    public bool IsActive { get; init; } = is_active;
    public Guid ParentId { get; init; } = parent_id;
}