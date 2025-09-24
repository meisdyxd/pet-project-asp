namespace DirectoryService.Contracts.Dtos.Departments;

public sealed record GetRootDepartmentsDto
{
    public GetRootDepartmentsDto(
        Guid id,
        string name,
        string identifier,
        DateTime createdAt,
        bool hasMoreChildren,
        List<ChildDepartmentDto> childs)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        CreatedAt = createdAt;
        HasMoreChildren = hasMoreChildren;
        Childs = childs;
    }
    
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Identifier { get; init; }
    public DateTime CreatedAt { get; init; }
    public bool HasMoreChildren { get; init; }
    public List<ChildDepartmentDto> Childs { get; init; } = [];
}