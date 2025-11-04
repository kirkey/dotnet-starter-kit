namespace Accounting.Application.BankReconciliations.Reject.v1;

/// <summary>
/// Validator for RejectBankReconciliationCommand.
/// Ensures required user information and reason are provided to reject a reconciliation.
/// </summary>
public sealed class RejectBankReconciliationCommandValidator : AbstractValidator<RejectBankReconciliationCommand>
{
    public RejectBankReconciliationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Reconciliation ID is required.")
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("Reconciliation ID must be a valid identifier.");

        RuleFor(x => x.RejectedBy)
            .NotEmpty()
            .WithMessage("RejectedBy is required.")
            .MaximumLength(256)
            .WithMessage("RejectedBy cannot exceed 256 characters.")
            .Matches(@"^[a-zA-Z0-9\s\-._@]{1,}$")
            .WithMessage("RejectedBy contains invalid characters. Only alphanumeric, spaces, hyphens, dots, and @ are allowed.");

        RuleFor(x => x.Reason)
            .MaximumLength(2048)
            .WithMessage("Rejection reason cannot exceed 2048 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason))
            .WithMessage("Rejection reason must not be only whitespace.")
            .Custom((reason, context) =>
            {
                if (!string.IsNullOrWhiteSpace(reason) && reason.Length < 5)
                {
                    context.AddFailure("Reason", "Rejection reason must be at least 5 characters when provided.");
                }
            });
    }
}

