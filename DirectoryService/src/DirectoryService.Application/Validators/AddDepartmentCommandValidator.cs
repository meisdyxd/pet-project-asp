using DirectoryService.Application.CQRS.Commands.AddDepartment;
using DirectoryService.Domain.ValueObjects.Department;
using DirectoryService.Application.Extensions;
using FluentValidation;

namespace DirectoryService.Application.Validators;

public class AddDepartmentCommandValidator : AbstractValidator<AddDepartmentCommand>
{
    public AddDepartmentCommandValidator()
    {
        RuleFor(r => r.Name)
            .MustBeValueObject(Name.Create);
        
        RuleFor(r => r.Identifier)
            .MustBeValueObject(Identifier.Create);
        
        RuleFor(r => r.LocationIds)
            .Must(l => l.Any())
            .WithError("locationIds", "LocationIds cannot be empty");
        
        RuleForEach(r => r.LocationIds)
            .NotEmpty()
            .WithError("locationIds", "locationIds are required");
            
        RuleFor(r => r.ParentId)
            .NotEmpty()
            .WithError("parentId", "parentId are required");
    }
}