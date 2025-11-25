namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.MarkAsAbsent.v1;

/// <summary>
/// Validator for MarkAsAbsentAttendanceCommand.
/// </summary>
public class MarkAsAbsentAttendanceValidator : AbstractValidator<MarkAsAbsentAttendanceCommand>
{
    public MarkAsAbsentAttendanceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Attendance record ID is required.");

        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("Reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));
    }
}

