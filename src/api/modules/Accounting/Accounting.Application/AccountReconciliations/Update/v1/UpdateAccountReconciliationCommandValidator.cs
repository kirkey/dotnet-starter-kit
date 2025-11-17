namespace Accounting.Application.AccountReconciliations.Update.v1;

/// <summary>
/// Validator for update command.
/// </summary>
public sealed class UpdateAccountReconciliationCommandValidator : AbstractValidator<UpdateAccountReconciliationCommand>
{
    public UpdateAccountReconciliationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Reconciliation ID is required.");

        RuleFor(x => x.GlBalance)
            .GreaterThanOrEqualTo(0).When(x => x.GlBalance.HasValue)
            .WithMessage("GL balance cannot be negative.");

        RuleFor(x => x.SubsidiaryLedgerBalance)
            .GreaterThanOrEqualTo(0).When(x => x.SubsidiaryLedgerBalance.HasValue)
            .WithMessage("Subsidiary ledger balance cannot be negative.");

        RuleFor(x => x.LineItemCount)
            .GreaterThanOrEqualTo(0).When(x => x.LineItemCount.HasValue)
            .WithMessage("Line item count cannot be negative.");
    }
}

