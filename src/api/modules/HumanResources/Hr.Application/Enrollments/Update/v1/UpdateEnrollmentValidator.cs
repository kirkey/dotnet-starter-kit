namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Update.v1;

/// <summary>
/// Validator for updating an enrollment.
/// </summary>
public class UpdateEnrollmentValidator : AbstractValidator<UpdateEnrollmentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEnrollmentValidator"/> class.
    /// </summary>
    public UpdateEnrollmentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Enrollment ID is required");

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

