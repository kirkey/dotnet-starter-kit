namespace FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Update.v1;

/// <summary>
/// Validator for updating shift assignments.
/// </summary>
public sealed class UpdateShiftAssignmentValidator : AbstractValidator<UpdateShiftAssignmentCommand>
{
    public UpdateShiftAssignmentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Shift assignment ID is required.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date.")
            .When(x => x.EndDate.HasValue && x.StartDate.HasValue);

        RuleFor(x => x.RecurringDayOfWeek)
            .InclusiveBetween(0, 6)
            .WithMessage("Day of week must be between 0 (Sunday) and 6 (Saturday).")
            .When(x => x.IsRecurring == true && x.RecurringDayOfWeek.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notes cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

