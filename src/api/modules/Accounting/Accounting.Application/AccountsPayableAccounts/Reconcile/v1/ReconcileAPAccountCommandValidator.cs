namespace Accounting.Application.AccountsPayableAccounts.Reconcile.v1;

public sealed class ReconcileAPAccountCommandValidator : AbstractValidator<ReconcileAPAccountCommand>
{
    public ReconcileAPAccountCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("AP account ID is required.");
        RuleFor(x => x.SubsidiaryLedgerBalance).LessThanOrEqualTo(999999999.99m).WithMessage("Balance must not exceed 999,999,999.99.")
            .GreaterThanOrEqualTo(-999999999.99m).WithMessage("Balance must not be less than -999,999,999.99.");
    }
}

