namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Update.v1;

/// <summary>
/// Validator for updating a benefit.
/// </summary>
public class UpdateBenefitValidator : AbstractValidator<UpdateBenefitCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateBenefitValidator"/> class.
    /// </summary>
    public UpdateBenefitValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Benefit ID is required");

        RuleFor(x => x.BenefitName)
            .MaximumLength(100)
            .WithMessage("Benefit name cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.BenefitName));

        RuleFor(x => x.EmployeeContribution)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Employee contribution cannot be negative")
            .When(x => x.EmployeeContribution.HasValue);

        RuleFor(x => x.EmployerContribution)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Employer contribution cannot be negative")
            .When(x => x.EmployerContribution.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

