using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using DirectoryService.Domain.ValueObjects.Position;

namespace DirectoryService.Domain;

public class Position : Entity<Guid>, ISoftDeletableEntity, IAuditableEntity
{
    protected Position(Guid id) : base(id) { }

    private readonly List<DepartmentPosition> _departmentPositions = [];

    public Name Name { get; private set; }
    public Description Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyList<DepartmentPosition> DepartmentPositions => _departmentPositions;

    private Position(Name name, Description description)
    {
        Name = name;
        Description = description;
        IsActive = Constants.CommonConstants.IS_ACTIVE_DEFAULT;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Result<Position, ErrorList> Create(Name name, Description description)
    {
        return new Position(name, description);
    }
}
