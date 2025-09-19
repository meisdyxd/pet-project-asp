using DirectoryService.Application.CQRS.Commands.Departments.UpdateLocations;
using DirectoryService.Application.Extensions;
using FluentValidation;

namespace DirectoryService.Application.Validators.DepartmentsValidators;

public class UpdateLocationsCommandValidator : AbstractValidator<UpdateLocationsCommand>
{
    public UpdateLocationsCommandValidator()
    {
        RuleFor(r => r.Request.LocationIds)
            .Must(l => l.Any())
            .WithError("locationIds", "LocationIds cannot be empty")
            .Must(l =>
            {
                var enumerable = l.ToList();
                return enumerable.Count == enumerable.Distinct().Count();
            });
        
        RuleForEach(r => r.Request.LocationIds)
            .NotEmpty()
            .WithError("locationIds", "locationIds are required");
    }
}