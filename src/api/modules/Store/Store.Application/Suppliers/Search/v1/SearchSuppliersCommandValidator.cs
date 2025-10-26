namespace FSH.Starter.WebApi.Store.Application.Suppliers.Search.v1;

/// <summary>
/// Validator for searching suppliers. Ensures valid pagination and filter sizes.
/// </summary>
public sealed class SearchSuppliersCommandValidator : AbstractValidator<SearchSuppliersCommand>
{
    public SearchSuppliersCommandValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).GreaterThan(0).LessThanOrEqualTo(500);
        RuleFor(x => x.Name).MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.Name));
        RuleFor(x => x.Code).MaximumLength(50).When(x => !string.IsNullOrWhiteSpace(x.Code));
    }
}

