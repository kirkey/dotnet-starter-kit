namespace FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Create.v1;

/// <summary>
/// Validator for creating shift assignments.
/// </summary>
public sealed class CreateShiftAssignmentValidator : AbstractValidator<CreateShiftAssignmentCommand>
{
    public CreateShiftAssignmentValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee ID is required.");

        RuleFor(x => x.ShiftId)
            .NotEmpty()
            .WithMessage("Shift ID is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date.")
            .When(x => x.EndDate.HasValue);

        RuleFor(x => x.RecurringDayOfWeek)
            .InclusiveBetween(0, 6)
            .WithMessage("Day of week must be between 0 (Sunday) and 6 (Saturday).")
            .When(x => x.IsRecurring && x.RecurringDayOfWeek.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(512)
            .WithMessage("Notes cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

