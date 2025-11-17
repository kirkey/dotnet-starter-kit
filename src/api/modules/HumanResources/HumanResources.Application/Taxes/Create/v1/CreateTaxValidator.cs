namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Create.v1;

/// <summary>
/// Validator for CreateTaxCommand.
/// Ensures all tax configuration parameters are valid before processing.
/// </summary>
public sealed class CreateTaxValidator : AbstractValidator<CreateTaxCommand>
{
    /// <summary>
    /// Initializes the validator with validation rules.
    /// </summary>
    public CreateTaxValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Tax code is required")
            .MaximumLength(50).WithMessage("Tax code cannot exceed 50 characters")
            .Matches(@"^[A-Z0-9\-_]+$").WithMessage("Tax code must be uppercase alphanumeric with hyphens/underscores only");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tax name is required")
            .MaximumLength(200).WithMessage("Tax name cannot exceed 200 characters");

        RuleFor(x => x.TaxType)
            .NotEmpty().WithMessage("Tax type is required")
            .Must(x => IsValidTaxType(x)).WithMessage("Invalid tax type. Must be one of: SalesTax, VAT, GST, UseTax, Excise, Withholding, Property, Other");

        RuleFor(x => x.Rate)
            .GreaterThanOrEqualTo(0).WithMessage("Tax rate cannot be negative")
            .LessThanOrEqualTo(1).WithMessage("Tax rate cannot exceed 100%");

        RuleFor(x => x.TaxCollectedAccountId)
            .NotEmpty().WithMessage("Tax collected account is required");

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(x => x.EffectiveDate ?? DateTime.MinValue)
            .WithMessage("Expiry date must be after effective date")
            .When(x => x.ExpiryDate.HasValue && x.EffectiveDate.HasValue);

        RuleFor(x => x.Jurisdiction)
            .MaximumLength(100).WithMessage("Jurisdiction cannot exceed 100 characters");

        RuleFor(x => x.TaxAuthority)
            .MaximumLength(200).WithMessage("Tax authority cannot exceed 200 characters");

        RuleFor(x => x.TaxRegistrationNumber)
            .MaximumLength(100).WithMessage("Tax registration number cannot exceed 100 characters");

        RuleFor(x => x.ReportingCategory)
            .MaximumLength(100).WithMessage("Reporting category cannot exceed 100 characters");
    }

    /// <summary>
    /// Validates that the tax type is one of the supported values.
    /// </summary>
    private static bool IsValidTaxType(string taxType)
    {
        var validTypes = new[] { "SalesTax", "VAT", "GST", "UseTax", "Excise", "Withholding", "Property", "Other" };
        return validTypes.Contains(taxType);
    }
}

