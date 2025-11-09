namespace Accounting.Application.AccountsReceivableAccounts.UpdateBalance.v1;

public sealed class UpdateArBalanceCommandValidator : AbstractValidator<UpdateArBalanceCommand>
{
    public UpdateArBalanceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("AR account ID is required.");
        RuleFor(x => x.Current0to30).GreaterThanOrEqualTo(0).WithMessage("Current 0-30 days must be non-negative.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Amount must not exceed 999,999,999.99.");
        RuleFor(x => x.Days31to60).GreaterThanOrEqualTo(0).WithMessage("Days 31-60 must be non-negative.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Amount must not exceed 999,999,999.99.");
        RuleFor(x => x.Days61to90).GreaterThanOrEqualTo(0).WithMessage("Days 61-90 must be non-negative.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Amount must not exceed 999,999,999.99.");
        RuleFor(x => x.Over90Days).GreaterThanOrEqualTo(0).WithMessage("Over 90 days must be non-negative.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Amount must not exceed 999,999,999.99.");
    }
}

