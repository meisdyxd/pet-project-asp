using DirectoryService.Application.CQRS.Commands.Departments.TransferDepartment;
using DirectoryService.Application.Extensions;
using FluentValidation;

namespace DirectoryService.Application.Validators.DepartmentsValidators;

public class TransferDepartmentCommandValidator : AbstractValidator<TransferDepartmentCommand>
{
    public TransferDepartmentCommandValidator()
    {
        RuleFor(c => c.Request.ParentId)
            .Must(c => c != default)
            .WithError("departmentId", "Department Id must be not empty")
            .When(c => c.Request.ParentId is not null);
    }
}
