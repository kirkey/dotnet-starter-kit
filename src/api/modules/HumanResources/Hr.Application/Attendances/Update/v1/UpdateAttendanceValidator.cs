namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Update.v1;

/// <summary>
/// Validator for updating an attendance record.
/// </summary>
public class UpdateAttendanceValidator : AbstractValidator<UpdateAttendanceCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateAttendanceValidator"/> class.
    /// </summary>
    public UpdateAttendanceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Attendance ID is required");

        RuleFor(x => x.ClockOutLocation)
            .MaximumLength(256)
            .WithMessage("Clock out location cannot exceed 250 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.ClockOutLocation));

        RuleFor(x => x.Status)
            .Must(BeValidStatus)
            .When(x => !string.IsNullOrWhiteSpace(x.Status))
            .WithMessage("Status must be Present, Late, Absent, or LeaveApproved");

        RuleFor(x => x.ManagerComment)
            .MaximumLength(512)
            .WithMessage("Manager comment cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.ManagerComment));
    }

    /// <summary>
    /// Validates if the status is valid.
    /// </summary>
    private static bool BeValidStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return true;

        var validStatuses = new[] { "Present", "Late", "Absent", "LeaveApproved" };
        return validStatuses.Contains(status);
    }
}

