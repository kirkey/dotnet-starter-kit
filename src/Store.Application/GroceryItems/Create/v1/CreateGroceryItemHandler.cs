using Store.Domain.Exceptions.GroceryItem;

namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Create.v1;

public sealed class CreateGroceryItemHandler(
    ILogger<CreateGroceryItemHandler> logger,
    [FromKeyedServices("store:grocery-items")] IRepository<GroceryItem> repository,
    [FromKeyedServices("store:grocery-items")] IReadRepository<GroceryItem> readRepository)
    : IRequestHandler<CreateGroceryItemCommand, CreateGroceryItemResponse>
{
    public async Task<CreateGroceryItemResponse> Handle(CreateGroceryItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate SKU
        var existingBySku = await readRepository.FirstOrDefaultAsync(new Specs.GroceryItemBySkuSpec(request.Sku!), cancellationToken).ConfigureAwait(false);
        if (existingBySku is not null)
            throw new DuplicateGroceryItemSkuException(request.Sku!);

        // Check for duplicate Barcode
        var existingByBarcode = await readRepository.FirstOrDefaultAsync(new Specs.GroceryItemByBarcodeSpec(request.Barcode!), cancellationToken).ConfigureAwait(false);
        if (existingByBarcode is not null)
            throw new DuplicateGroceryItemBarcodeException(request.Barcode!);

        var groceryItem = GroceryItem.Create(
            request.Name!,
            request.Description,
            request.Sku!,
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

        await repository.AddAsync(groceryItem, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("grocery item created {GroceryItemId}", groceryItem.Id);
        return new CreateGroceryItemResponse(groceryItem.Id);
    }
}
