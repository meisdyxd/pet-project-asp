namespace DirectoryService.Domain;

public class DepartmentLocation
{
    protected DepartmentLocation()
    {
        
    }

    public Guid DepartmentId { get; set; }
    public Guid LocationId { get; set; }

    public Department Department { get; set; } = null!;
    public Location Location { get; set; } = null!;
}