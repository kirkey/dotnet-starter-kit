namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.ClockOut.v1;

/// <summary>
/// Validator for ClockOutAttendanceCommand.
/// </summary>
public class ClockOutAttendanceValidator : AbstractValidator<ClockOutAttendanceCommand>
{
    public ClockOutAttendanceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Attendance record ID is required.");

        RuleFor(x => x.ClockOutTime)
            .NotEmpty().WithMessage("Clock out time is required.")
            .GreaterThan(TimeSpan.Zero).WithMessage("Clock out time must be greater than zero.");

        RuleFor(x => x.ClockOutLocation)
            .MaximumLength(256).WithMessage("Clock out location must not exceed 250 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ClockOutLocation));

        RuleFor(x => x.Notes)
            .MaximumLength(512).WithMessage("Notes must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

