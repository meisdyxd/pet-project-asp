using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Errors;
using DirectoryService.Domain.ValueObjects.Department;
using Path = DirectoryService.Domain.ValueObjects.Department.Path;

namespace DirectoryService.Domain;

public class Department: Entity<Guid>, ISoftDeletableEntity, IAuditableEntity
{
    protected Department(Guid id): base(id) { }

    private readonly List<DepartmentLocation> _departmentLocations = [];
    private readonly List<DepartmentPosition> _departmentPositions = [];
    private readonly List<Department> _childrenDepartments = [];

    public Name Name { get; private set; }
    public Identifier Identifier { get; private set; }
    public Path Path { get; private set; }
    public short Depth { get; private set; }
    public bool IsActive { get; private set; }
    public Guid? ParentId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Department? Parent { get; private set; }
    public IReadOnlyList<Department> ChildrenDepartments => _childrenDepartments;
    public IReadOnlyList<DepartmentLocation> DepartmentLocations => _departmentLocations;
    public IReadOnlyList<DepartmentPosition> DepartmentPositions => _departmentPositions;


    private Department(
        Name name, 
        Identifier identifier,
        Path path,
        short depth, 
        Guid? parent = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Identifier = identifier;
        ParentId = parent;
        Path = path;
        Depth = depth;
        IsActive = Constants.CommonConstants.IS_ACTIVE_DEFAULT;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Result<Department, ErrorList> Create(
        Name name,
        Identifier identifier,
        Path path,
        short depth,
        Guid? parent = null)
    {
        return new Department(
            name, 
            identifier, 
            path, 
            depth, 
            parent);
    }

    public void AddLocations(IEnumerable<Guid> locations)
    {
        var departmentLocations = locations
            .Select(l => DepartmentLocation.Create(l, Id).Value);
        _departmentLocations.AddRange(departmentLocations);
    }

    public void UpdateLocations(IEnumerable<Guid> locations)
    {
        _departmentLocations.Clear();
        AddLocations(locations);
    }
}
