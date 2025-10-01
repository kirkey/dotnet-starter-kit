namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Add.v1;

public class AddInventoryTransferItemCommandValidator : AbstractValidator<AddInventoryTransferItemCommand>
{
    public AddInventoryTransferItemCommandValidator(
        [FromKeyedServices("store:inventory-transfers")] IReadRepository<InventoryTransfer> transferReadRepository,
        [FromKeyedServices("store:grocery-items")] IReadRepository<GroceryItem> groceryReadRepository)
    {
        RuleFor(x => x.InventoryTransferId).NotEmpty().WithMessage("InventoryTransferId is required");
        RuleFor(x => x.GroceryItemId).NotEmpty().WithMessage("GroceryItemId is required");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero");
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("Unit price cannot be negative");

        // Ensure transfer exists and is Pending
        RuleFor(x => x.InventoryTransferId).MustAsync(async (id, ct) =>
        {
            var transfer = await transferReadRepository.GetByIdAsync(id, ct).ConfigureAwait(false);
            return transfer is not null && string.Equals(transfer.Status, "Pending", StringComparison.OrdinalIgnoreCase);
        }).WithMessage("Inventory transfer not found or not in a modifiable state (Pending required)");

        // Ensure grocery item exists
        RuleFor(x => x.GroceryItemId).MustAsync(async (id, ct) =>
        {
            var gi = await groceryReadRepository.GetByIdAsync(id, ct).ConfigureAwait(false);
            return gi is not null;
        }).WithMessage("Grocery item not found");

        // Prevent duplicate grocery item in the same transfer
        RuleFor(x => x).MustAsync(async (cmd, ct) =>
        {
            var transfer = await transferReadRepository.GetByIdAsync(cmd.InventoryTransferId, ct).ConfigureAwait(false);
            if (transfer is null) return true; // handled by previous rule
            return transfer.Items.All(i => i.GroceryItemId != cmd.GroceryItemId);
        }).WithMessage("The grocery item already exists in the inventory transfer");
    }
}
