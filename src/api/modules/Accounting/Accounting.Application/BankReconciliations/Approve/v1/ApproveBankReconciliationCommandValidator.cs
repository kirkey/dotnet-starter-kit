namespace Accounting.Application.BankReconciliations.Approve.v1;

/// <summary>
/// Validator for ApproveBankReconciliationCommand.
/// Ensures required user information is provided to approve a reconciliation.
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

        RuleFor(x => x.ApprovedBy)
            .NotEmpty()
            .WithMessage("ApprovedBy is required.")
            .MaximumLength(256)
            .WithMessage("ApprovedBy cannot exceed 256 characters.")
            .Matches(@"^[a-zA-Z0-9\s\-._@]{1,}$")
            .WithMessage("ApprovedBy contains invalid characters. Only alphanumeric, spaces, hyphens, dots, and @ are allowed.");
    }
}

