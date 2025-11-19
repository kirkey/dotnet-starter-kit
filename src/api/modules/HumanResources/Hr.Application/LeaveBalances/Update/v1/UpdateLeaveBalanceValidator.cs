namespace FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Update.v1;

public class UpdateLeaveBalanceValidator : AbstractValidator<UpdateLeaveBalanceCommand>
{
    public UpdateLeaveBalanceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Leave balance ID is required");

        RuleFor(x => x.AccruedDays)
            .GreaterThanOrEqualTo(0).WithMessage("Accrued days cannot be negative")
            .When(x => x.AccruedDays.HasValue);

        RuleFor(x => x.TakenDays)
            .GreaterThanOrEqualTo(0).WithMessage("Taken days cannot be negative")
            .When(x => x.TakenDays.HasValue);
    }
}

