using DirectoryService.Application.CQRS.Commands.Departments.ChangeParent;
using DirectoryService.Application.Extensions;
using FluentValidation;

namespace DirectoryService.Application.Validators.DepartmentsValidators;

public class ChangeParentCommandValidator : AbstractValidator<ChangeParentCommand>
{
    public ChangeParentCommandValidator()
    {
        RuleFor(c => c.Request.ParentId)
            .Must(c => c != default)
            .WithError("departmentId", "Department Id must be not empty")
            .When(c => c.Request.ParentId is not null);
    }
}
