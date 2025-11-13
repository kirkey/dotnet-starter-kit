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

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(200).WithMessage("Company name must not exceed 200 characters.");

        RuleFor(c => c.Tin)
            .MaximumLength(50).WithMessage("TIN must not exceed 50 characters.")
            .When(c => !string.IsNullOrWhiteSpace(c.Tin));
    }
}

