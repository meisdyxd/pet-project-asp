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

    public static Result<DepartmentLocation, Error> Create(Guid locationId, Guid departmentId)
    {
        if (locationId == Guid.Empty)
            return Errors.InvalidValue.Default(nameof(locationId));
        if (departmentId == Guid.Empty)
            return Errors.InvalidValue.Default(nameof(departmentId));
        
        return new DepartmentLocation(locationId, departmentId);
    }
}