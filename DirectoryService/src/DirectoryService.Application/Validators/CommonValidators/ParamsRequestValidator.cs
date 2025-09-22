using DirectoryService.Application.Extensions;
using DirectoryService.Contracts.Enums;
using DirectoryService.Contracts.Requests.CommonRequests;
using FluentValidation;

namespace DirectoryService.Application.Validators.CommonValidators;

public class ParamsRequestValidator : AbstractValidator<ParamsRequest>
{
    public ParamsRequestValidator()
    {
        RuleFor(r => r.Search)
            .NotEmpty()
            .WithError("search", "search parameter is empty")
            .When(r => r.Search != null);

        RuleFor(r => r.Page)
            .Must(p => p > 0)
            .WithError("page", "Page must be greater than 0");

        RuleFor(r => r.PageSize)
            .Must(ps => ps > 0)
            .WithError("pageSize", "Page size must be greater than 0");

        RuleFor(r => r.SortDirection)
            .Must(sd => Enum.TryParse<SortDirectionsEnum>(sd.ToUpper(), out _))
            .WithError("sortDirection", $"Sort direction must be 'ASC' or 'DESC'")
            .When(r => r.SortDirection != null);
    }
}
