namespace DirectoryService.Contracts;

public interface ISoftDeletableEntity
{
    public bool IsActive { get; }
}