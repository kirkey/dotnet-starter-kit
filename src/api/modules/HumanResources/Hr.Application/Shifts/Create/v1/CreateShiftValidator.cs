namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Create.v1;

public class CreateShiftValidator : AbstractValidator<CreateShiftCommand>
{
    public CreateShiftValidator()
    {
        RuleFor(x => x.ShiftName)
            .NotEmpty()
            .WithMessage("Shift name is required")
            .MaximumLength(128)
            .WithMessage("Shift name cannot exceed 100 characters");

        RuleFor(x => x.StartTime)
            .NotEmpty()
            .WithMessage("Start time is required");

        RuleFor(x => x.EndTime)
            .NotEmpty()
            .WithMessage("End time is required")
            .Must((cmd, endTime) => endTime != cmd.StartTime || cmd.IsOvernight)
            .WithMessage("End time must be different from start time");

        RuleFor(x => x.BreakDurationMinutes)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Break duration cannot be negative");

        RuleFor(x => x.Description)
            .MaximumLength(512)
            .WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

