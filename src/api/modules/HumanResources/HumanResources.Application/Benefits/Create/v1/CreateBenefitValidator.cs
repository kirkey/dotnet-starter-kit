namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Create.v1;

/// <summary>
/// Validator for creating a benefit.
/// </summary>
public class CreateBenefitValidator : AbstractValidator<CreateBenefitCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateBenefitValidator"/> class.
    /// </summary>
    public CreateBenefitValidator()
    {
        RuleFor(x => x.BenefitName)
            .NotEmpty()
            .WithMessage("Benefit name is required")
            .MaximumLength(100)
            .WithMessage("Benefit name cannot exceed 100 characters");

        RuleFor(x => x.BenefitType)
            .NotEmpty()
            .WithMessage("Benefit type is required")
            .MaximumLength(50)
            .WithMessage("Benefit type cannot exceed 50 characters");

        RuleFor(x => x.EmployeeContribution)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Employee contribution cannot be negative");

        RuleFor(x => x.EmployerContribution)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Employer contribution cannot be negative");

        RuleFor(x => x.AnnualLimit)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Annual limit cannot be negative")
            .When(x => x.AnnualLimit.HasValue);

        RuleFor(x => x.MinimumEligibleEmployees)
            .GreaterThan(0)
            .WithMessage("Minimum eligible employees must be greater than 0")
            .When(x => x.MinimumEligibleEmployees.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

