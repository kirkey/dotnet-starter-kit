namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.MarkAsLate.v1;

/// <summary>
/// Validator for MarkAsLateAttendanceCommand.
/// </summary>
public class MarkAsLateAttendanceValidator : AbstractValidator<MarkAsLateAttendanceCommand>
{
    public MarkAsLateAttendanceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Attendance record ID is required.");

        RuleFor(x => x.MinutesLate)
            .GreaterThan(0).WithMessage("Minutes late must be greater than zero.");

        RuleFor(x => x.Reason)
            .MaximumLength(512).WithMessage("Reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));
    }
}

