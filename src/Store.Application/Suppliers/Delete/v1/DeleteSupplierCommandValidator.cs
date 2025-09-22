using FSH.Starter.WebApi.Store.Application.GroceryItems.Specs;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Specs;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Delete.v1;

/// <summary>
/// Validates that a supplier can be deleted by ensuring it exists and has no dependent references.
/// </summary>
public sealed class DeleteSupplierCommandValidator : AbstractValidator<DeleteSupplierCommand>
{
    public DeleteSupplierCommandValidator(
        [FromKeyedServices("store:suppliers")] IReadRepository<Supplier> suppliers,
        [FromKeyedServices("store:grocery-items")] IReadRepository<GroceryItem> groceryItems,
        [FromKeyedServices("store:purchase-orders")] IReadRepository<PurchaseOrder> purchaseOrders)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(async (id, ct) => await suppliers.GetByIdAsync(id, ct).ConfigureAwait(false) is not null)
            .WithMessage("Supplier not found.")
            .MustAsync(async (id, ct) =>
            {
                // No grocery items should reference this supplier
                var hasItems = await groceryItems.AnyAsync(new GroceryItemsBySupplierIdSpec(id), ct).ConfigureAwait(false);
                return !hasItems;
            }).WithMessage("Cannot delete a supplier that has grocery items.")
            .MustAsync(async (id, ct) =>
            {
                // No purchase orders should reference this supplier
                var hasOrders = await purchaseOrders.AnyAsync(new PurchaseOrdersBySupplierIdSpec(id), ct).ConfigureAwait(false);
                return !hasOrders;
            }).WithMessage("Cannot delete a supplier that has purchase orders.");
    }
}
