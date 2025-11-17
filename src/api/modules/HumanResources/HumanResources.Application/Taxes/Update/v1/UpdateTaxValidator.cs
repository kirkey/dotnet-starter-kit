namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Update.v1;

/// <summary>
/// Validator for UpdateTaxCommand.
/// Ensures all update parameters are valid before processing.
/// </summary>
public sealed class UpdateTaxValidator : AbstractValidator<UpdateTaxCommand>
{
    /// <summary>
    /// Initializes the validator with validation rules.
    /// </summary>
    public UpdateTaxValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Tax ID is required");

        RuleFor(x => x.Name)
            .MaximumLength(200).WithMessage("Tax name cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.TaxType)
            .Must(x => string.IsNullOrEmpty(x) || IsValidTaxType(x))
            .WithMessage("Invalid tax type. Must be one of: SalesTax, VAT, GST, UseTax, Excise, Withholding, Property, Other");

        RuleFor(x => x.Rate)
            .GreaterThanOrEqualTo(0).WithMessage("Tax rate cannot be negative")
            .LessThanOrEqualTo(1).WithMessage("Tax rate cannot exceed 100%")
            .When(x => x.Rate.HasValue);

        RuleFor(x => x.Jurisdiction)
            .MaximumLength(100).WithMessage("Jurisdiction cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Jurisdiction));

        RuleFor(x => x.TaxAuthority)
            .MaximumLength(200).WithMessage("Tax authority cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.TaxAuthority));

        RuleFor(x => x.TaxRegistrationNumber)
            .MaximumLength(100).WithMessage("Tax registration number cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.TaxRegistrationNumber));

        RuleFor(x => x.ReportingCategory)
            .MaximumLength(100).WithMessage("Reporting category cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.ReportingCategory));
    }

    /// <summary>
    /// Validates that the tax type is one of the supported values.
    /// </summary>
    private static bool IsValidTaxType(string? taxType)
    {
        if (string.IsNullOrEmpty(taxType))
            return true;

        var validTypes = new[] { "SalesTax", "VAT", "GST", "UseTax", "Excise", "Withholding", "Property", "Other" };
        return validTypes.Contains(taxType);
    }
}

