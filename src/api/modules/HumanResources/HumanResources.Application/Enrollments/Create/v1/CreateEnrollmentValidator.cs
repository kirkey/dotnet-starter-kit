namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Create.v1;

/// <summary>
/// Validator for creating an enrollment.
/// </summary>
public class CreateEnrollmentValidator : AbstractValidator<CreateEnrollmentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEnrollmentValidator"/> class.
    /// </summary>
    public CreateEnrollmentValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee ID is required");

        RuleFor(x => x.BenefitId)
            .NotEmpty()
            .WithMessage("Benefit ID is required");

        RuleFor(x => x.EnrollmentDate)
            .NotEmpty()
            .WithMessage("Enrollment date is required")
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Enrollment date cannot be in the future");

        RuleFor(x => x.EffectiveDate)
            .NotEmpty()
            .WithMessage("Effective date is required")
            .GreaterThanOrEqualTo(x => x.EnrollmentDate)
            .WithMessage("Effective date must be on or after enrollment date");

        RuleFor(x => x.CoverageLevel)
            .MaximumLength(50)
            .WithMessage("Coverage level cannot exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.CoverageLevel));

        RuleFor(x => x.EmployeeContributionAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Employee contribution cannot be negative")
            .When(x => x.EmployeeContributionAmount.HasValue);

        RuleFor(x => x.EmployerContributionAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Employer contribution cannot be negative")
            .When(x => x.EmployerContributionAmount.HasValue);
    }
}

