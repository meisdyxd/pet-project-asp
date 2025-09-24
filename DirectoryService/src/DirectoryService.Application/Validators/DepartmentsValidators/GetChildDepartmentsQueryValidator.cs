using DirectoryService.Application.CQRS.Queries.Departments.GetChildDepartments;
using DirectoryService.Application.CQRS.Queries.Departments.GetRootDepartmentsWithChilds;
using DirectoryService.Application.Extensions;
using FluentValidation;

namespace DirectoryService.Application.Validators.DepartmentsValidators;

public class GetChildDepartmentsQueryValidator : AbstractValidator<GetChildDepartmentsQuery>
{
    public GetChildDepartmentsQueryValidator()
    {
        RuleFor(q => q.Page)
            .Must(p => p > 0)
            .WithError("page", "Page must be greater than 0.");
        
        RuleFor(q => q.PageSize)
            .Must(p => p > 0)
            .WithError("pageSize", "PageSize must be greater than 0.");
        
        RuleFor(q => q.ParentId)
            .NotEmpty()
            .WithError("parentId", "Parent id cannot be empty.");
    }
}