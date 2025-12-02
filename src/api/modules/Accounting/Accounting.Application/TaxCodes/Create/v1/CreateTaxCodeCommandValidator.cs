using Accounting.Application.TaxCodes.Specs;

namespace Accounting.Application.TaxCodes.Create.v1;

/// <summary>
/// Validator for the CreateTaxCodeCommand with comprehensive validation rules.
/// Ensures data integrity and business rule compliance before tax code creation.
/// </summary>
public class CreateTaxCodeCommandValidator : AbstractValidator<CreateTaxCodeCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTaxCodeCommandValidator"/> class.
    /// </summary>
    /// <param name="repository">Repository for tax code data access to check for duplicates.</param>
    public CreateTaxCodeCommandValidator(
        IReadRepository<TaxCode> repository)
    {
        // Code validation
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Tax code is required.")
            .MaximumLength(32).WithMessage("Tax code must not exceed 20 characters.")
            .MinimumLength(2).WithMessage("Tax code must be at least 2 characters long.")
            .Matches(@"^[A-Z0-9\-_]+$").WithMessage("Tax code must contain only uppercase letters, numbers, hyphens, and underscores.")
            .MustAsync(async (code, cancellationToken) =>
            {
                var spec = new TaxCodeByCodeSpec(code);
                return await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false) == null;
            }).WithMessage("Tax code '{PropertyValue}' already exists.");

        // Name validation
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tax name is required.")
            .MaximumLength(256).WithMessage("Tax name must not exceed 256 characters.")
            .MinimumLength(2).WithMessage("Tax name must be at least 2 characters long.");

        // TaxType validation
        RuleFor(x => x.TaxType)
            .NotEmpty().WithMessage("Tax type is required.")
            .Must(taxType => Enum.TryParse<TaxType>(taxType, true, out _))
            .WithMessage("Invalid tax type. Valid values: SalesTax, VAT, GST, UseTax, Excise, Withholding, Property, Other.");

        // Rate validation - expecting decimal between 0 and 1 (0% to 100%)
        RuleFor(x => x.Rate)
            .InclusiveBetween(0m, 1m).WithMessage("Tax rate must be between 0 and 1 (0% to 100%).");

        // TaxCollectedAccountId validation
        RuleFor(x => x.TaxCollectedAccountId)
            .NotEmpty().WithMessage("Tax collected account is required for tracking tax remittance obligations.");

        // EffectiveDate validation
        RuleFor(x => x.EffectiveDate)
            .NotEmpty().WithMessage("Effective date is required.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date.AddDays(-1))
            .WithMessage("Effective date cannot be more than one day in the past.");

        // ExpiryDate validation
        RuleFor(x => x.ExpiryDate)
            .GreaterThan(x => x.EffectiveDate)
            .WithMessage("Expiry date must be after the effective date.")
            .When(x => x.ExpiryDate.HasValue);

        // Jurisdiction validation
        RuleFor(x => x.Jurisdiction)
            .MaximumLength(128).WithMessage("Jurisdiction must not exceed 128 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Jurisdiction));

        // TaxAuthority validation
        RuleFor(x => x.TaxAuthority)
            .MaximumLength(256).WithMessage("Tax authority must not exceed 256 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.TaxAuthority));

        // TaxRegistrationNumber validation
        RuleFor(x => x.TaxRegistrationNumber)
            .MaximumLength(64).WithMessage("Tax registration number must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.TaxRegistrationNumber));

        // ReportingCategory validation
        RuleFor(x => x.ReportingCategory)
            .MaximumLength(128).WithMessage("Reporting category must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ReportingCategory));

        // Description validation
        RuleFor(x => x.Description)
            .MaximumLength(2048).WithMessage("Description must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

