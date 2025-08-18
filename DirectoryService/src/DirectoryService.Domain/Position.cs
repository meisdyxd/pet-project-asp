using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using DirectoryService.Domain.ValueObjects.Position;

namespace DirectoryService.Domain;

public class Position : Entity<Guid>, ISoftDeletableEntity, IAuditableEntity
{
    private readonly List<Department> _departments = [];

    public Name Name { get; private set; }
    public Description Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyList<Department> Departments => _departments;

    private Position(Name name, Description description)
    {
        Name = name;
        Description = description;
        IsActive = Constants.CommonConstants.IS_ACTIVE_DEFAULT;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Result<Position, Error> Create(Name name, Description description)
    {
        return new Position(name, description);
    }
}
