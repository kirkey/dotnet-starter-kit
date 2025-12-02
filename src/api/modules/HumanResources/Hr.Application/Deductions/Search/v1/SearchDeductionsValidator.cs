namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Search.v1;

/// <summary>
/// Validator for SearchDeductionsRequest.
/// </summary>
public sealed class SearchDeductionsValidator : AbstractValidator<SearchDeductionsRequest>
{
    public SearchDeductionsValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageNumber must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(256)
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));
    }
}

