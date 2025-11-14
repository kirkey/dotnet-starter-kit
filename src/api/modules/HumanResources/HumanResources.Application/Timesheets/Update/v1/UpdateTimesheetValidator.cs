namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Update.v1;

public class UpdateTimesheetValidator : AbstractValidator<UpdateTimesheetCommand>
{
    public UpdateTimesheetValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Timesheet ID is required");

        RuleFor(x => x.RegularHours)
            .GreaterThanOrEqualTo(0)
            .When(x => x.RegularHours.HasValue)
            .WithMessage("Regular hours cannot be negative");

        RuleFor(x => x.OvertimeHours)
            .GreaterThanOrEqualTo(0)
            .When(x => x.OvertimeHours.HasValue)
            .WithMessage("Overtime hours cannot be negative");

        RuleFor(x => x.Status)
            .Must(BeValidStatus)
            .When(x => !string.IsNullOrWhiteSpace(x.Status))
            .WithMessage("Status must be Draft, Submitted, Approved, or Rejected");
    }

    private static bool BeValidStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return true;

        var validStatuses = new[] { "Draft", "Submitted", "Approved", "Rejected", "Locked" };
        return validStatuses.Contains(status);
    }
}

