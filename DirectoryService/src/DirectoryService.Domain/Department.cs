using CSharpFunctionalExtensions;
using DirectoryService.Contracts;

namespace DirectoryService.Domain;

public class Department: Entity<Guid>, ISoftDeletableEntity, IAuditableEntity
{
    public string Name { get; set; } = null!;
    public string Identifier { get; set; } = null!;
    public Guid? ParentId { get; set; }
    
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
}