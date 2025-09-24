namespace DirectoryService.Contracts.Dtos.Departments;

public sealed record ChildDepartmentDto
{
    public ChildDepartmentDto(
        Guid id, 
        string name, 
        string identifier,
        DateTime createdAt)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        CreatedAt = createdAt;
    }

    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Identifier { get; init; }
    public DateTime CreatedAt { get; init; }
}