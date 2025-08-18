namespace DirectoryService.Contracts;

public interface IAuditableEntity
{
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }
}