using DirectoryService.Application.CQRS.Commands.AddPosition;
using DirectoryService.Application.Extensions;
using DirectoryService.Domain.ValueObjects.Position;
using FluentValidation;

namespace DirectoryService.Application.Validators;

public class AddPositionCommandValidator : AbstractValidator<AddPositionCommand>
{
    public AddPositionCommandValidator()
    {
        RuleFor(r => r.Name)
            .MustBeValueObject(Name.Create);
        
        RuleFor(r => r.Description)
            .MustBeValueObject(Description.Create);
        
        RuleFor(r => r.DepartmentIds)
            .Must(r => r.Any())
            .WithError("departmentIds", "departmentIds cannot be empty")
            .Must(r =>
            {
                var enumerable = r.ToList();
                return enumerable.Distinct().Count() == enumerable.Count;
            })
            .WithError("departmentIds", "departmentIds must be unique");
    }
}