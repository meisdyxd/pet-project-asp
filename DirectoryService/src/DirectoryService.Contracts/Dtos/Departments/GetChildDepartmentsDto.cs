namespace DirectoryService.Contracts.Dtos.Departments;

public class GetChildDepartmentsDto
{
    public GetChildDepartmentsDto(
        Guid id,
        string name,
        string identifier,
        DateTime createdAt,
        bool hasMoreChildren)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        CreatedAt = createdAt;
        HasMoreChildren = hasMoreChildren;
    }

    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Identifier { get; init; }
    public DateTime CreatedAt { get; init; }
    public bool HasMoreChildren { get; init; }
}