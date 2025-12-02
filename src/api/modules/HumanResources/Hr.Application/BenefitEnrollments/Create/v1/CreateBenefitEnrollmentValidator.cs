namespace FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Create.v1;

/// <summary>
/// Validator for CreateBenefitEnrollmentCommand.
/// </summary>
public class CreateBenefitEnrollmentValidator : AbstractValidator<CreateBenefitEnrollmentCommand>
{
    public CreateBenefitEnrollmentValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.BenefitId)
            .NotEmpty().WithMessage("Benefit ID is required.");

        RuleFor(x => x.CoverageLevel)
            .NotEmpty().WithMessage("Coverage level is required.")
            .MaximumLength(64).WithMessage("Coverage level must not exceed 50 characters.");

        RuleFor(x => x.EmployeeContributionAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Employee contribution cannot be negative.");

        RuleFor(x => x.EmployerContributionAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Employer contribution cannot be negative.");

        RuleFor(x => x.EffectiveDate)
            .GreaterThanOrEqualTo(x => x.EnrollmentDate ?? DateTime.UtcNow)
            .WithMessage("Effective date must be after enrollment date.")
            .When(x => x.EffectiveDate.HasValue && x.EnrollmentDate.HasValue);
    }
}

