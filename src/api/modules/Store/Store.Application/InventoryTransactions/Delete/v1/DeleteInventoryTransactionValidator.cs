namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Delete.v1;

public class DeleteInventoryTransactionValidator : AbstractValidator<DeleteInventoryTransactionCommand>
{
    public DeleteInventoryTransactionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Transaction ID is required.");
    }
}
