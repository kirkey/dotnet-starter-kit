namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.MarkAsLeave.v1;

/// <summary>
/// Validator for MarkAsLeaveAttendanceCommand.
/// </summary>
public class MarkAsLeaveAttendanceValidator : AbstractValidator<MarkAsLeaveAttendanceCommand>
{
    public MarkAsLeaveAttendanceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Attendance record ID is required.");

        RuleFor(x => x.Reason)
            .MaximumLength(512).WithMessage("Reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));
    }
}

