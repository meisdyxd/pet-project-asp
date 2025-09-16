using DirectoryService.Application.CQRS.Commands.Positions.AddPosition;
using DirectoryService.Application.Extensions;
using DirectoryService.Domain.ValueObjects.Position;
using FluentValidation;

namespace DirectoryService.Application.Validators.PositionsValidators;

public class AddPositionCommandValidator : AbstractValidator<AddPositionCommand>
{
    public AddPositionCommandValidator()
    {
        RuleFor(r => r.Request.Name)
            .MustBeValueObject(Name.Create);
        
        RuleFor(r => r.Request.Description)
            .MustBeValueObject(Description.Create);
        
        RuleFor(r => r.Request.DepartmentIds)
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