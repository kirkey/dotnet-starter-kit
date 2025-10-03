namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Add.v1;

public class AddInventoryTransferItemCommandValidator : AbstractValidator<AddInventoryTransferItemCommand>
{
    public AddInventoryTransferItemCommandValidator(
        [FromKeyedServices("store:inventory-transfers")] IReadRepository<InventoryTransfer> transferReadRepository,
        [FromKeyedServices("store:items")] IReadRepository<Item> itemRepository)
    {
        RuleFor(x => x.InventoryTransferId).NotEmpty().WithMessage("InventoryTransferId is required");
        RuleFor(x => x.ItemId).NotEmpty().WithMessage("ItemId is required");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero");
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("Unit price cannot be negative");

        // Ensure transfer exists and is Pending
        RuleFor(x => x.InventoryTransferId).MustAsync(async (id, ct) =>
        {
            var transfer = await transferReadRepository.GetByIdAsync(id, ct).ConfigureAwait(false);
            return transfer is not null && string.Equals(transfer.Status, "Pending", StringComparison.OrdinalIgnoreCase);
        }).WithMessage("Inventory transfer not found or not in a modifiable state (Pending required)");

        // Ensure item exists
        RuleFor(x => x.ItemId).MustAsync(async (id, ct) =>
        {
            var item = await itemRepository.GetByIdAsync(id, ct).ConfigureAwait(false);
            return item is not null;
        }).WithMessage("Item not found");

        // Prevent duplicate item in the same transfer
        RuleFor(x => x).MustAsync(async (cmd, ct) =>
        {
            var transfer = await transferReadRepository.GetByIdAsync(cmd.InventoryTransferId, ct).ConfigureAwait(false);
            if (transfer is null) return true; // handled by previous rule
            return transfer.Items.All(i => i.ItemId != cmd.ItemId);
        }).WithMessage("The item already exists in the inventory transfer");
    }
}
