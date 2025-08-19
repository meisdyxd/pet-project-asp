using CSharpFunctionalExtensions;
using DirectoryService.Contracts;

namespace DirectoryService.Domain;

public class DepartmentPosition
{
    protected DepartmentPosition() { }
    
    private DepartmentPosition(Guid positionId, Guid departmentId)
    {
        PositionId = positionId;
        DepartmentId = departmentId;
    }

    public Guid DepartmentId { get; private set; }
    public Guid PositionId { get; private set; }

    public Department Department { get; private set; }
    public Position Position { get; private set; }
    
    public static Result<DepartmentPosition, Error> Create(Guid positionId, Guid departmentId)
    {
        if (positionId == Guid.Empty)
            return Errors.InvalidValue.Default(nameof(positionId));
        if (departmentId == Guid.Empty)
            return Errors.InvalidValue.Default(nameof(departmentId));
        
        return new DepartmentPosition(positionId, departmentId);
    }
}