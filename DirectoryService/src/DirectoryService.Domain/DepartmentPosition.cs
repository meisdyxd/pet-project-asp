namespace DirectoryService.Domain;

public class DepartmentPosition
{
    protected DepartmentPosition()
    {

    }

    public Guid DepartmentId { get; set; }
    public Guid PositionId { get; set; }

    public Department Department { get; set; }
    public Position Position { get; set; }
}