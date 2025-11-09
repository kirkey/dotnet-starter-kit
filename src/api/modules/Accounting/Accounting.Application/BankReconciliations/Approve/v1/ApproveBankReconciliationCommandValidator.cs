namespace Accounting.Application.BankReconciliations.Approve.v1;

/// <summary>
/// Validator for ApproveBankReconciliationCommand.
/// The approver is automatically determined from the current user session.
/// </summary>
public sealed class ApproveBankReconciliationCommandValidator : AbstractValidator<ApproveBankReconciliationCommand>
{
    public ApproveBankReconciliationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Reconciliation ID is required.")
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("Reconciliation ID must be a valid identifier.");
    }
}

