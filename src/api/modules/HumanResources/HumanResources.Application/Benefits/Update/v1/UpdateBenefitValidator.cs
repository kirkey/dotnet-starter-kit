namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Update.v1;

/// <summary>
/// Validator for UpdateBenefitCommand.
/// </summary>
public class UpdateBenefitValidator : AbstractValidator<UpdateBenefitCommand>
{
    public UpdateBenefitValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Benefit ID is required.");

        RuleFor(x => x.EmployeeContribution)
            .GreaterThanOrEqualTo(0).WithMessage("Employee contribution cannot be negative.")
            .When(x => x.EmployeeContribution.HasValue);

        RuleFor(x => x.EmployerContribution)
            .GreaterThanOrEqualTo(0).WithMessage("Employer contribution cannot be negative.")
            .When(x => x.EmployerContribution.HasValue);

        RuleFor(x => x.CoverageType)
            .MaximumLength(50).WithMessage("Coverage type must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.CoverageType));

        RuleFor(x => x.ProviderName)
            .MaximumLength(100).WithMessage("Provider name must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ProviderName));

        RuleFor(x => x.CoverageAmount)
            .GreaterThan(0).WithMessage("Coverage amount must be greater than 0.")
            .When(x => x.CoverageAmount.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

