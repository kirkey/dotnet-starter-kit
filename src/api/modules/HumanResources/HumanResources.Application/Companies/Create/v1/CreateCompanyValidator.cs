using FluentValidation;

namespace FSH.Starter.WebApi.HumanResources.Application.Companies.Create.v1;

/// <summary>
/// Validator for CreateCompanyCommand.
/// </summary>
public class CreateCompanyValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyValidator()
    {
        RuleFor(c => c.CompanyCode)
            .NotEmpty().WithMessage("Company code is required.")
            .MaximumLength(20).WithMessage("Company code must not exceed 20 characters.")
            .Matches(@"^[A-Z0-9-]+$").WithMessage("Company code must contain only uppercase letters, numbers, and hyphens.");

        RuleFor(c => c.LegalName)
            .NotEmpty().WithMessage("Legal name is required.")
            .MaximumLength(200).WithMessage("Legal name must not exceed 200 characters.");

        RuleFor(c => c.TradeName)
            .MaximumLength(200).WithMessage("Trade name must not exceed 200 characters.")
            .When(c => !string.IsNullOrWhiteSpace(c.TradeName));

        RuleFor(c => c.TaxId)
            .MaximumLength(50).WithMessage("Tax ID must not exceed 50 characters.")
            .When(c => !string.IsNullOrWhiteSpace(c.TaxId));

        RuleFor(c => c.BaseCurrency)
            .NotEmpty().WithMessage("Base currency is required.")
            .Length(3).WithMessage("Base currency must be a 3-letter ISO currency code.")
            .Matches(@"^[A-Z]{3}$").WithMessage("Base currency must be a valid 3-letter ISO currency code.");

        RuleFor(c => c.FiscalYearEnd)
            .InclusiveBetween(1, 12).WithMessage("Fiscal year end must be between 1 and 12.");

        RuleFor(c => c.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(c => !string.IsNullOrWhiteSpace(c.Description));

        RuleFor(c => c.Notes)
            .MaximumLength(2000).WithMessage("Notes must not exceed 2000 characters.")
            .When(c => !string.IsNullOrWhiteSpace(c.Notes));
    }
}

