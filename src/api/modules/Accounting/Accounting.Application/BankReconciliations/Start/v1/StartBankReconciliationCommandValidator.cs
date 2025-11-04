namespace Accounting.Application.BankReconciliations.Start.v1;

/// <summary>
/// Validator for StartBankReconciliationCommand.
/// Ensures a valid reconciliation ID is provided.
/// </summary>
public sealed class StartBankReconciliationCommandValidator : AbstractValidator<StartBankReconciliationCommand>
{
    public StartBankReconciliationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Reconciliation ID is required.")
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("Reconciliation ID must be a valid identifier.");
    }
}

