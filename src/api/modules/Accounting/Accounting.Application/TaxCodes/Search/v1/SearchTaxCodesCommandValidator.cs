namespace Accounting.Application.TaxCodes.Search.v1;

/// <summary>
/// Validator for the SearchTaxCodesCommand.
/// Ensures search parameters are valid and within acceptable ranges.
/// </summary>
public class SearchTaxCodesCommandValidator : AbstractValidator<SearchTaxCodesCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchTaxCodesCommandValidator"/> class.
    /// </summary>
    public SearchTaxCodesCommandValidator()
    {
        // PageNumber validation
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        // PageSize validation
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.")
            .LessThanOrEqualTo(500).WithMessage("Page size must not exceed 500.");

        // Code validation
        RuleFor(x => x.Code)
            .MaximumLength(32).WithMessage("Tax code search term must not exceed 20 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Code));

        // TaxType validation
        RuleFor(x => x.TaxType)
            .Must(taxType => Enum.TryParse<TaxType>(taxType, true, out _))
            .WithMessage("Invalid tax type. Valid values: SalesTax, VAT, GST, UseTax, Excise, Withholding, Property, Other.")
            .When(x => !string.IsNullOrWhiteSpace(x.TaxType));

        // Jurisdiction validation
        RuleFor(x => x.Jurisdiction)
            .MaximumLength(128).WithMessage("Jurisdiction search term must not exceed 128 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Jurisdiction));
    }
}

