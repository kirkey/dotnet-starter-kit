namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Terminate.v1;

/// <summary>
/// Validator for terminating an enrollment.
/// </summary>
public class TerminateEnrollmentValidator : AbstractValidator<TerminateEnrollmentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TerminateEnrollmentValidator"/> class.
    /// </summary>
    public TerminateEnrollmentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Enrollment ID is required");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required")
            .GreaterThan(DateTime.Today)
            .WithMessage("End date must be in the future");
    }
}

