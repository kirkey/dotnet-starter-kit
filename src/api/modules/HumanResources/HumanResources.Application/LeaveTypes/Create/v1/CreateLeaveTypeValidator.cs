namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Create.v1;

public class CreateLeaveTypeValidator : AbstractValidator<CreateLeaveTypeCommand>
{
    public CreateLeaveTypeValidator()
    {
        RuleFor(x => x.LeaveName)
            .NotEmpty().WithMessage("Leave name is required")
            .MaximumLength(100).WithMessage("Leave name cannot exceed 100 characters");

        RuleFor(x => x.AnnualAllowance)
            .GreaterThan(0).WithMessage("Annual allowance must be greater than 0");

        RuleFor(x => x.AccrualFrequency)
            .Must(BeValidFrequency).WithMessage("Accrual frequency must be Monthly, Quarterly, or Annual");

        RuleFor(x => x.MaxCarryoverDays)
            .GreaterThanOrEqualTo(0).WithMessage("Max carryover days cannot be negative");

        RuleFor(x => x.MinimumNoticeDay)
            .GreaterThan(0).WithMessage("Minimum notice days must be greater than 0")
            .When(x => x.MinimumNoticeDay.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }

    private static bool BeValidFrequency(string? frequency)
    {
        if (string.IsNullOrWhiteSpace(frequency))
            return false;

        var validFrequencies = new[] { "Monthly", "Quarterly", "Annual" };
        return validFrequencies.Contains(frequency);
    }
}

