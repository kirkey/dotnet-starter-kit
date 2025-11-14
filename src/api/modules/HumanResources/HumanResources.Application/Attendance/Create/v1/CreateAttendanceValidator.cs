namespace FSH.Starter.WebApi.HumanResources.Application.Attendance.Create.v1;

public class CreateAttendanceValidator : AbstractValidator<CreateAttendanceCommand>
{
    public CreateAttendanceValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.AttendanceDate)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Attendance date cannot be in the future.")
            .GreaterThanOrEqualTo(DateTime.Today.AddYears(-1)).WithMessage("Attendance date cannot be more than 1 year old.");

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
}

