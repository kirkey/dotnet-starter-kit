using Store.Domain.Exceptions.GroceryItem;

namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Update.v1;

public sealed class UpdateGroceryItemHandler(
    ILogger<UpdateGroceryItemHandler> logger,
    [FromKeyedServices("store:grocery-items")] IRepository<GroceryItem> repository,
    [FromKeyedServices("store:grocery-items")] IReadRepository<GroceryItem> readRepository)
    : IRequestHandler<UpdateGroceryItemCommand, UpdateGroceryItemResponse>
{
    public async Task<UpdateGroceryItemResponse> Handle(UpdateGroceryItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var groceryItem = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = groceryItem ?? throw new GroceryItemNotFoundException(request.Id);

        // Check SKU uniqueness (other than this item)
        if (!string.IsNullOrWhiteSpace(request.SKU))
        {
            var otherWithSku = await readRepository.FirstOrDefaultAsync(new Specs.GroceryItemBySkuSpec(request.SKU!), cancellationToken).ConfigureAwait(false);
            if (otherWithSku is not null && otherWithSku.Id != request.Id)
                throw new DuplicateGroceryItemSkuException(request.SKU!);
        }

        // Check Barcode uniqueness (other than this item)
        if (!string.IsNullOrWhiteSpace(request.Barcode))
        {
            var otherWithBarcode = await readRepository.FirstOrDefaultAsync(new Specs.GroceryItemByBarcodeSpec(request.Barcode!), cancellationToken).ConfigureAwait(false);
            if (otherWithBarcode is not null && otherWithBarcode.Id != request.Id)
                throw new DuplicateGroceryItemBarcodeException(request.Barcode!);
        }

        var updated = groceryItem.Update(
            request.Name,
            request.Description,
            request.SKU!,
            request.Barcode!,
            request.Price,
            request.Cost,
            request.MinimumStock,
            request.MaximumStock,
            request.CurrentStock,
            request.ReorderPoint,
            request.IsPerishable,
            request.ExpiryDate,
            request.Brand,
            request.Manufacturer,
            request.Weight,
            request.WeightUnit,
            request.CategoryId,
            request.SupplierId,
            request.WarehouseLocationId);

        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("grocery item with id : {GroceryItemId} updated.", groceryItem.Id);
        return new UpdateGroceryItemResponse(groceryItem.Id);
    }
}
