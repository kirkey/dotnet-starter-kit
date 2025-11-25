namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Reject.v1;

/// <summary>
/// Validator for RejectAttendanceCommand.
/// </summary>
public class RejectAttendanceValidator : AbstractValidator<RejectAttendanceCommand>
{
    public RejectAttendanceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Attendance record ID is required.");

        RuleFor(x => x.Comment)
            .MaximumLength(500).WithMessage("Comment must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Comment));
    }
}

