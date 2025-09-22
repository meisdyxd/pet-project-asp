using DirectoryService.Application.CQRS.Queries.Locations.GetLocations;
using DirectoryService.Application.Enums.LocationEnums;
using DirectoryService.Application.Extensions;
using DirectoryService.Application.Validators.CommonValidators;
using FluentValidation;

namespace DirectoryService.Application.Validators.LocationsValidators;

public class GetLocationsQueryValidator : AbstractValidator<GetLocationsQuery>
{
    public GetLocationsQueryValidator()
    {
        RuleForEach(q => q.DepartmentIds)
            .Must(id => id != default)
            .WithError("departmentIds", "Department Id is default value")
            .When(q => q.DepartmentIds != null);

        RuleFor(q => q.Request)
            .SetValidator(new ParamsRequestValidator());

        RuleForEach(q => q.Request.SortBy)
            .Must(sb => Enum.TryParse<GetLocationsSortFields>(sb, true, out _))
            .WithError("sortBy", "Sort by field is must be " + string.Join(", ", Enum.GetNames<GetLocationsSortFields>()))
            .When(q => q.Request.SortBy != null);
    }
}
