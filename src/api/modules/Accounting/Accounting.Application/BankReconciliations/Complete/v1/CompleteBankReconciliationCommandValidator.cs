namespace Accounting.Application.BankReconciliations.Complete.v1;

/// <summary>
/// Validator for CompleteBankReconciliationCommand.
/// Ensures required user information is provided to mark reconciliation as completed.
/// </summary>
public sealed class CompleteBankReconciliationCommandValidator : AbstractValidator<CompleteBankReconciliationCommand>
{
    public CompleteBankReconciliationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Reconciliation ID is required.")
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("Reconciliation ID must be a valid identifier.");

        RuleFor(x => x.ReconciledBy)
            .NotEmpty()
            .WithMessage("ReconciledBy is required.")
            .MaximumLength(256)
            .WithMessage("ReconciledBy cannot exceed 256 characters.")
            .Matches(@"^[a-zA-Z0-9\s\-._@]{1,}$")
            .WithMessage("ReconciledBy contains invalid characters. Only alphanumeric, spaces, hyphens, dots, and @ are allowed.");
    }
}

