namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Update.v1;

public class UpdateInventoryTransferItemCommandValidator : AbstractValidator<UpdateInventoryTransferItemCommand>
{
    public UpdateInventoryTransferItemCommandValidator([FromKeyedServices("store:inventory-transfers")] IReadRepository<InventoryTransfer> transferReadRepository)
    {
        RuleFor(x => x.InventoryTransferId).NotEmpty().WithMessage("InventoryTransferId is required");
        RuleFor(x => x.ItemId).NotEmpty().WithMessage("ItemId is required");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero");
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("UnitPrice cannot be negative");

        // Ensure transfer exists and is Pending
        RuleFor(x => x.InventoryTransferId).MustAsync(async (id, ct) =>
        {
            var transfer = await transferReadRepository.GetByIdAsync(id, ct).ConfigureAwait(false);
            return transfer is not null && string.Equals(transfer.Status, "Pending", StringComparison.OrdinalIgnoreCase);
        }).WithMessage("Inventory transfer not found or not in a modifiable state (Pending required)");

        // Ensure the item exists in the transfer
        RuleFor(x => x).MustAsync(async (cmd, ct) =>
        {
            var transfer = await transferReadRepository.GetByIdAsync(cmd.InventoryTransferId, ct).ConfigureAwait(false);
            if (transfer is null) return true; // handled by previous rule
            return transfer.Items.Any(i => i.Id == cmd.ItemId);
        }).WithMessage("Inventory transfer item not found in the specified transfer");
    }
}
