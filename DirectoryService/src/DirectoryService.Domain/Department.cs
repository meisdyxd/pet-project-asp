using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using DirectoryService.Domain.ValueObjects.Department;
using Path = DirectoryService.Domain.ValueObjects.Department.Path;

namespace DirectoryService.Domain;

public class Department: Entity<Guid>, ISoftDeletableEntity, IAuditableEntity
{
    private readonly List<Location> _locations = [];
    private readonly List<Position> _positions = [];
    private readonly List<Department> _childrenDepartments = [];

    public Name Name { get; private set; }
    public Identifier Identifier { get; private set; }
    public Department? Parent { get; private set; }
    public Path Path { get; private set; }
    public short Depth { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyList<Department> ChildrenDepartments => _childrenDepartments;
    public IReadOnlyList<Location> Locations => _locations;
    public IReadOnlyList<Position> Positions => _positions;


    private Department(
        Name name, 
        Identifier identifier,
        Path path,
        short depth, 
        Department? parent = null)
    {
        Name = name;
        Identifier = identifier;
        Parent = parent;
        Path = path;
        Depth = depth;
        IsActive = Constants.CommonConstants.IS_ACTIVE_DEFAULT;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Result<Department, Error> Create(
        Name name,
        Identifier identifier,
        Path path,
        short depth,
        Department? parent = null)
    {
        return new Department(
            name, 
            identifier, 
            path, 
            depth, 
            parent);
    }
}
