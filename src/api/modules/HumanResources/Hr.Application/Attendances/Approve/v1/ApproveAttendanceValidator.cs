namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Approve.v1;

/// <summary>
/// Validator for ApproveAttendanceCommand.
/// </summary>
public class ApproveAttendanceValidator : AbstractValidator<ApproveAttendanceCommand>
{
    public ApproveAttendanceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Attendance record ID is required.");

        RuleFor(x => x.Comment)
            .MaximumLength(512).WithMessage("Comment must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Comment));
    }
}

