namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Update.v1;

public class UpdateLeaveTypeValidator : AbstractValidator<UpdateLeaveTypeCommand>
{
    public UpdateLeaveTypeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Leave type ID is required");

        RuleFor(x => x.AnnualAllowance)
            .GreaterThan(0).WithMessage("Annual allowance must be greater than 0")
            .When(x => x.AnnualAllowance.HasValue);

        RuleFor(x => x.MaxCarryoverDays)
            .GreaterThanOrEqualTo(0).WithMessage("Max carryover days cannot be negative")
            .When(x => x.MaxCarryoverDays.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

