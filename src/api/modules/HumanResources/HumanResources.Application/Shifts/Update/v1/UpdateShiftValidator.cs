namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Update.v1;

public class UpdateShiftValidator : AbstractValidator<UpdateShiftCommand>
{
    public UpdateShiftValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(x => x.ShiftName)
            .MaximumLength(100).WithMessage("Shift name must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ShiftName));

        RuleFor(x => x.StartTime)
            .Must(BeValidTime).WithMessage("Start time must be valid (00:00:00 - 23:59:59)")
            .When(x => x.StartTime.HasValue);

        RuleFor(x => x.EndTime)
            .Must(BeValidTime).WithMessage("End time must be valid (00:00:00 - 23:59:59)")
            .When(x => x.EndTime.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }

    private static bool BeValidTime(TimeSpan? time)
    {
        return !time.HasValue || (time.Value >= TimeSpan.Zero && time.Value < TimeSpan.FromHours(24));
    }
}

