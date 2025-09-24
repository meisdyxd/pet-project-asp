using DirectoryService.Application.CQRS.Queries.Departments.GetRootDepartmentsWithChilds;
using DirectoryService.Application.Extensions;
using FluentValidation;

namespace DirectoryService.Application.Validators.DepartmentsValidators;

public class GetRootDepartmentsWithChildsValidator : AbstractValidator<GetRootDepartmentsWithChildsQuery>
{
    public GetRootDepartmentsWithChildsValidator()
    {
        RuleFor(q => q.Page)
            .Must(p => p > 0)
            .WithError("page", "Page amount must be greater than 0.");
        
        RuleFor(q => q.PageSize)
            .Must(p => p > 0)
            .WithError("pageSize", "PageSize amount must be greater than 0.");
        
        RuleFor(q => q.Prefetch)
            .Must(p => p > 0)
            .WithError("prefetch", "Prefetch amount must be greater than 0.");
    }
}