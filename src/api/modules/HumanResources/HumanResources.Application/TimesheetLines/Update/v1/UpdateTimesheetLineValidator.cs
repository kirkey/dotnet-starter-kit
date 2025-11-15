namespace FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Update.v1;

/// <summary>
/// Validator for updating a timesheet line.
/// </summary>
public sealed class UpdateTimesheetLineValidator : AbstractValidator<UpdateTimesheetLineCommand>
{
    public UpdateTimesheetLineValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Timesheet line ID is required.");

        When(x => x.RegularHours.HasValue, () =>
        {
            RuleFor(x => x.RegularHours!.Value)
                .GreaterThanOrEqualTo(0).WithMessage("Regular hours cannot be negative.")
                .LessThanOrEqualTo(24).WithMessage("Regular hours cannot exceed 24.");
        });

        When(x => x.OvertimeHours.HasValue, () =>
        {
            RuleFor(x => x.OvertimeHours!.Value)
                .GreaterThanOrEqualTo(0).WithMessage("Overtime hours cannot be negative.")
                .LessThanOrEqualTo(24).WithMessage("Overtime hours cannot exceed 24.");
        });

        RuleFor(x => x.ProjectId)
            .MaximumLength(50).WithMessage("Project ID cannot exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ProjectId));

        RuleFor(x => x.TaskDescription)
            .MaximumLength(500).WithMessage("Task description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.TaskDescription));

        RuleFor(x => x.BillingRate)
            .GreaterThanOrEqualTo(0).WithMessage("Billing rate cannot be negative.")
            .When(x => x.BillingRate.HasValue);
    }
}

