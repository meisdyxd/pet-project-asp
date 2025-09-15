using System.Net;
using CSharpFunctionalExtensions;
using DirectoryService.Contracts;

namespace DirectoryService.Domain;

public class DepartmentLocation
{
    protected DepartmentLocation() { }

    private DepartmentLocation(Guid locationId, Guid departmentId)
    {
        LocationId = locationId;
        DepartmentId = departmentId;
    }
    
    public Guid DepartmentId { get; private set; }
    public Guid LocationId { get; private set; }
    
    public Department Department { get; private set; }
    public Location Location { get; private set; }

    public static Result<DepartmentLocation, ErrorList> Create(Guid locationId, Guid departmentId)
    {
        var errors = new List<Error>();
        
        if (locationId == Guid.Empty)
            errors.Add(Errors.InvalidValue.Default(nameof(locationId)));
        if (departmentId == Guid.Empty)
            errors.Add(Errors.InvalidValue.Default(nameof(departmentId)));

        if (errors.Count != 0)
            return new ErrorList(errors, HttpStatusCode.BadRequest);
        
        return new DepartmentLocation(locationId, departmentId);
    }
}