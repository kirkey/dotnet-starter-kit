namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Search.v1;

/// <summary>
/// Validator for SearchBenefitsRequest.
/// </summary>
public sealed class SearchBenefitsValidator : AbstractValidator<SearchBenefitsRequest>
{
    public SearchBenefitsValidator()
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

