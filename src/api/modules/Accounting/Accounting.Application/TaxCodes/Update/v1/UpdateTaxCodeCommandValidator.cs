namespace Accounting.Application.TaxCodes.Update.v1;

/// <summary>
/// Validator for the UpdateTaxCodeCommand with comprehensive validation rules.
/// Ensures data integrity and business rule compliance before tax code update.
/// </summary>
public class UpdateTaxCodeCommandValidator : AbstractValidator<UpdateTaxCodeCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTaxCodeCommandValidator"/> class.
    /// </summary>
    public UpdateTaxCodeCommandValidator()
    {
        // Id validation
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Tax code ID is required.");

        // Name validation
        RuleFor(x => x.Name)
            .MaximumLength(256).WithMessage("Tax name must not exceed 256 characters.")
            .MinimumLength(2).WithMessage("Tax name must be at least 2 characters long.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

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
            .MaximumLength(50).WithMessage("Tax registration number must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.TaxRegistrationNumber));

        // ReportingCategory validation
        RuleFor(x => x.ReportingCategory)
            .MaximumLength(100).WithMessage("Reporting category must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ReportingCategory));

        // Description validation
        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

