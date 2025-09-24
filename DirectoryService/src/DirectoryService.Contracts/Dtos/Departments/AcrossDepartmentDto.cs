namespace DirectoryService.Contracts.Dtos.Departments;

public record AcrossDepartmentDto
{
    public AcrossDepartmentDto(
        Guid id, 
        string name,
        string identifier,
        Guid? parentId,
        DateTime createdAt,
        bool hasMoreChildren)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        CreatedAt = createdAt;
        HasMoreChildren = hasMoreChildren;
    }

    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Identifier { get; init; }
    public Guid? ParentId { get; init; }
    public DateTime CreatedAt { get; init; }
    public bool HasMoreChildren { get; init; }
}