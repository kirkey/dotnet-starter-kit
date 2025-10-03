namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Approve.v1;

public class ApproveInventoryTransactionValidator : AbstractValidator<ApproveInventoryTransactionCommand>
{
    public ApproveInventoryTransactionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Transaction ID is required.");

        RuleFor(x => x.ApprovedBy)
            .NotEmpty().WithMessage("Approved by is required.")
            .MaximumLength(100).WithMessage("Approved by must not exceed 100 characters.");
    }
}
