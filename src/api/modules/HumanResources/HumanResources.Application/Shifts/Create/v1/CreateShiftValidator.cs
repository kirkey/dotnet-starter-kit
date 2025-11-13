namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Create.v1;

public class CreateShiftValidator : AbstractValidator<CreateShiftCommand>
{
    public CreateShiftValidator()
    {
        RuleFor(x => x.ShiftName)
            .NotEmpty().WithMessage("Shift name is required.")
            .MaximumLength(100).WithMessage("Shift name must not exceed 100 characters.");

        RuleFor(x => x.StartTime)
            .Must(BeValidTime).WithMessage("Start time must be valid (00:00:00 - 23:59:59)");

        RuleFor(x => x.EndTime)
            .Must(BeValidTime).WithMessage("End time must be valid (00:00:00 - 23:59:59)")
            .Custom((endTime, context) =>
            {
                var startTime = context.InstanceToValidate.StartTime;
                var isOvernight = context.InstanceToValidate.IsOvernight;
                if (startTime >= endTime && !isOvernight)
                    context.AddFailure("End time must be after start time.");
            });

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }

    private static bool BeValidTime(TimeSpan time)
    {
        return time >= TimeSpan.Zero && time < TimeSpan.FromHours(24);
    }
}

