namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Create.v1;

/// <summary>
/// Validator for CreateBenefitCommand.
/// </summary>
public class CreateBenefitValidator : AbstractValidator<CreateBenefitCommand>
{
    public CreateBenefitValidator()
    {
        RuleFor(x => x.BenefitName)
            .NotEmpty().WithMessage("Benefit name is required.")
            .MaximumLength(128).WithMessage("Benefit name must not exceed 100 characters.");

        RuleFor(x => x.BenefitType)
            .NotEmpty().WithMessage("Benefit type is required.")
            .MaximumLength(64).WithMessage("Benefit type must not exceed 50 characters.");

        RuleFor(x => x.EmployeeContribution)
            .GreaterThanOrEqualTo(0).WithMessage("Employee contribution cannot be negative.");

        RuleFor(x => x.EmployerContribution)
            .GreaterThanOrEqualTo(0).WithMessage("Employer contribution cannot be negative.");

        RuleFor(x => x.CoverageType)
            .MaximumLength(64).WithMessage("Coverage type must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.CoverageType));

        RuleFor(x => x.ProviderName)
            .MaximumLength(128).WithMessage("Provider name must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ProviderName));

        RuleFor(x => x.CoverageAmount)
            .GreaterThan(0).WithMessage("Coverage amount must be greater than 0.")
            .When(x => x.CoverageAmount.HasValue);

        RuleFor(x => x.WaitingPeriodDays)
            .GreaterThanOrEqualTo(0).WithMessage("Waiting period cannot be negative.")
            .When(x => x.WaitingPeriodDays.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(512).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

