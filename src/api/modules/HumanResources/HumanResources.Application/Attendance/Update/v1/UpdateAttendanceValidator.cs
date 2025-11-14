namespace FSH.Starter.WebApi.HumanResources.Application.Attendance.Update.v1;

public class UpdateAttendanceValidator : AbstractValidator<UpdateAttendanceCommand>
{
    public UpdateAttendanceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(x => x.ClockInTime)
            .Must(BeValidTime).WithMessage("Clock in time must be between 00:00:00 and 23:59:59.")
            .When(x => x.ClockInTime.HasValue);

        RuleFor(x => x.ClockOutTime)
            .Must(BeValidTime).WithMessage("Clock out time must be between 00:00:00 and 23:59:59.")
            .When(x => x.ClockOutTime.HasValue);

        RuleFor(x => x)
            .Custom((request, context) =>
            {
                if (request.ClockInTime.HasValue && request.ClockOutTime.HasValue)
                {
                    if (request.ClockOutTime.Value <= request.ClockInTime.Value)
                    {
                        context.AddFailure("Clock out time must be after clock in time.");
                    }
                }
            });

        RuleFor(x => x.Status)
            .Must(BeValidStatus).WithMessage("Status must be one of: Present, Late, Absent, LeaveApproved.")
            .When(x => !string.IsNullOrWhiteSpace(x.Status));

        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("Reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));

        RuleFor(x => x.ClockInLocation)
            .MaximumLength(500).WithMessage("Clock in location must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ClockInLocation));

        RuleFor(x => x.ClockOutLocation)
            .MaximumLength(500).WithMessage("Clock out location must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ClockOutLocation));
    }

    private static bool BeValidTime(TimeSpan? time)
    {
        if (!time.HasValue)
            return true;

        return time.Value >= TimeSpan.Zero && time.Value < TimeSpan.FromHours(24);
    }

    private static bool BeValidStatus(string? status)
    {
        return status is "Present" or "Late" or "Absent" or "LeaveApproved";
    }
}

