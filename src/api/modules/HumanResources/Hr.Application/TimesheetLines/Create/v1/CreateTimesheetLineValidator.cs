namespace FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Create.v1;

/// <summary>
/// Validator for creating timesheet line.
/// </summary>
public sealed class CreateTimesheetLineValidator : AbstractValidator<CreateTimesheetLineCommand>
{
    public CreateTimesheetLineValidator()
    {
        RuleFor(x => x.TimesheetId).NotEmpty().WithMessage("Timesheet ID is required.");
        RuleFor(x => x.WorkDate).NotEmpty().WithMessage("Work date is required.");
        RuleFor(x => x.RegularHours)
            .GreaterThanOrEqualTo(0).WithMessage("Regular hours cannot be negative.")
            .LessThanOrEqualTo(24).WithMessage("Regular hours cannot exceed 24.");
        RuleFor(x => x.OvertimeHours)
            .GreaterThanOrEqualTo(0).WithMessage("Overtime hours cannot be negative.")
            .LessThanOrEqualTo(24).WithMessage("Overtime hours cannot exceed 24.");
        RuleFor(x => x)
            .Must(x => x.RegularHours + x.OvertimeHours <= 24)
            .WithMessage("Total hours (regular + overtime) cannot exceed 24 hours per day.");
        RuleFor(x => x.ProjectId)
            .MaximumLength(64).WithMessage("Project ID cannot exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ProjectId));
        RuleFor(x => x.TaskDescription)
            .MaximumLength(512).WithMessage("Task description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.TaskDescription));
        RuleFor(x => x.BillingRate)
            .GreaterThanOrEqualTo(0).WithMessage("Billing rate cannot be negative.")
            .When(x => x.BillingRate.HasValue);
    }
}

