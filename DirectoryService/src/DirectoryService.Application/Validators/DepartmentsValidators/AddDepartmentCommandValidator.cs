using DirectoryService.Application.CQRS.Commands.Departments.AddDepartment;
using DirectoryService.Application.Extensions;
using DirectoryService.Domain.ValueObjects.Department;
using FluentValidation;

namespace DirectoryService.Application.Validators.DepartmentsValidators;

public class AddDepartmentCommandValidator : AbstractValidator<AddDepartmentCommand>
{
    public AddDepartmentCommandValidator()
    {
        RuleFor(r => r.Request.Name)
            .MustBeValueObject(Name.Create);
        
        RuleFor(r => r.Request.Identifier)
            .MustBeValueObject(Identifier.Create);
        
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
            
        RuleFor(r => r.Request.ParentId)
            .NotEmpty()
            .WithError("parentId", "parentId are required")
            .When(r => r.Request.ParentId != null);
    }
}