using System.Net;
using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Errors;

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
    
    public static Result<DepartmentPosition, ErrorList> Create(Guid positionId, Guid departmentId)
    {
        var errors = new List<Error>();
        
        if (positionId == Guid.Empty)
            errors.Add(Errors.InvalidValue.Default(nameof(positionId)));
        if (departmentId == Guid.Empty)
            errors.Add(Errors.InvalidValue.Default(nameof(departmentId)));
        
        if (errors.Count != 0)
            return new ErrorList(errors, HttpStatusCode.BadRequest);
        
        return new DepartmentPosition(positionId, departmentId);
    }
}