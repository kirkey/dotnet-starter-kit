namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Reject.v1;

/// <summary>
/// Validator for RejectInventoryTransactionCommand.
/// </summary>
public class RejectInventoryTransactionValidator : AbstractValidator<RejectInventoryTransactionCommand>
{
    public RejectInventoryTransactionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Transaction ID is required.");

        RuleFor(x => x.RejectedBy)
            .NotEmpty()
            .WithMessage("Rejected by is required.")
            .MaximumLength(100)
            .WithMessage("Rejected by must not exceed 100 characters.");

        RuleFor(x => x.RejectionReason)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.RejectionReason))
            .WithMessage("Rejection reason must not exceed 500 characters.");
    }
}

