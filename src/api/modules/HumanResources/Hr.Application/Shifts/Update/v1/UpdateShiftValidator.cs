namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Update.v1;

public class UpdateShiftValidator : AbstractValidator<UpdateShiftCommand>
{
    public UpdateShiftValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Shift ID is required");

        RuleFor(x => x.ShiftName)
            .MaximumLength(128)
            .WithMessage("Shift name cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.ShiftName));

        RuleFor(x => x.BreakDurationMinutes)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Break duration cannot be negative")
            .When(x => x.BreakDurationMinutes.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(512)
            .WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

