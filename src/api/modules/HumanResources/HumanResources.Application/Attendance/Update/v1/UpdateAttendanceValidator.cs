namespace FSH.Starter.WebApi.HumanResources.Application.Attendance.Update.v1;

public class UpdateAttendanceValidator : AbstractValidator<UpdateAttendanceCommand>
{
    public UpdateAttendanceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(x => x.ClockInTime)
            .Must(BeValidTime).WithMessage("Clock in time must be valid (00:00:00 - 23:59:59)")
            .When(x => x.ClockInTime.HasValue);

        RuleFor(x => x.ClockOutTime)
            .Must(BeValidTime).WithMessage("Clock out time must be valid (00:00:00 - 23:59:59)")
            .When(x => x.ClockOutTime.HasValue);

        RuleFor(x => x.Status)
            .Must(BeValidStatus).WithMessage("Status must be one of: Present, Late, Absent, LeaveApproved, HalfDay.")
            .When(x => !string.IsNullOrWhiteSpace(x.Status));

        RuleFor(x => x.MinutesLate)
            .GreaterThanOrEqualTo(0).WithMessage("Minutes late must be greater than or equal to 0.")
            .When(x => x.MinutesLate.HasValue);

        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("Reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));

        RuleFor(x => x.ManagerComment)
            .MaximumLength(500).WithMessage("Manager comment must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ManagerComment));
    }

    private static bool BeValidTime(TimeSpan? time)
    {
        return !time.HasValue || (time.Value >= TimeSpan.Zero && time.Value < TimeSpan.FromHours(24));
    }

    private static bool BeValidStatus(string status)
    {
        return status is "Present" or "Late" or "Absent" or "LeaveApproved" or "HalfDay";
    }
}

