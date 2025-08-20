using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using DirectoryService.Domain.ValueObjects.Common;
using DirectoryService.Domain.ValueObjects.Location;

namespace DirectoryService.Domain;

public class Location: Entity<Guid>, ISoftDeletableEntity, IAuditableEntity
{
    protected Location(Guid id) : base(id) { }

    private readonly List<DepartmentLocation> _departmentsLocations = [];

    public Name Name { get; private set; } 
    public Address Address { get; private set; }
    public IANATimezone Timezone { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyList<DepartmentLocation> DepartmentLocations => _departmentsLocations;

    private Location(
    Name name,
    Address address,
    IANATimezone timezone)
    {
        Name = name;
        Address = address;
        Timezone = timezone;
        IsActive = Constants.CommonConstants.IS_ACTIVE_DEFAULT;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Result<Location, Error> Create(
        Name name, 
        Address address, 
        IANATimezone timezone)
    {
        return new Location(
            name,
            address,
            timezone);
    }
}
