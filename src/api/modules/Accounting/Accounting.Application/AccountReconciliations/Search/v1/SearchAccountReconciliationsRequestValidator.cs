namespace Accounting.Application.AccountReconciliations.Search.v1;

/// <summary>
/// Validator for search request.
/// </summary>
public sealed class SearchAccountReconciliationsRequestValidator : AbstractValidator<SearchAccountReconciliationsRequest>
{
    public SearchAccountReconciliationsRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}

