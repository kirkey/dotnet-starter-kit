namespace Accounting.Application.AccountReconciliations.Create.v1;

/// <summary>
/// Validator for CreateAccountReconciliationCommand.
/// </summary>
public sealed class CreateAccountReconciliationCommandValidator : AbstractValidator<CreateAccountReconciliationCommand>
{
    public CreateAccountReconciliationCommandValidator()
    {
        RuleFor(x => x.GeneralLedgerAccountId)
            .NotEmpty().WithMessage("GL account is required.");

        RuleFor(x => x.AccountingPeriodId)
            .NotEmpty().WithMessage("Accounting period is required.");

        RuleFor(x => x.GlBalance)
            .GreaterThanOrEqualTo(0).WithMessage("GL balance cannot be negative.");

        RuleFor(x => x.SubsidiaryLedgerBalance)
            .GreaterThanOrEqualTo(0).WithMessage("Subsidiary ledger balance cannot be negative.");

        RuleFor(x => x.SubsidiaryLedgerSource)
            .NotEmpty().WithMessage("Subsidiary ledger source is required.")
            .MaximumLength(128).WithMessage("Source cannot exceed 100 characters.");

        RuleFor(x => x.ReconciliationDate)
            .NotEmpty().WithMessage("Reconciliation date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Reconciliation date cannot be in the future.");

        RuleFor(x => x.VarianceExplanation)
            .MaximumLength(1024).WithMessage("Variance explanation cannot exceed 1000 characters.");
    }
}

