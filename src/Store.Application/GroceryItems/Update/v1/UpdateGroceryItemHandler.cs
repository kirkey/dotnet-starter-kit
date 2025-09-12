using Store.Domain.Exceptions.GroceryItem;

namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Update.v1;

public sealed class UpdateGroceryItemHandler(
    ILogger<UpdateGroceryItemHandler> logger,
    [FromKeyedServices("store:grocery-items")] IRepository<GroceryItem> repository)
    : IRequestHandler<UpdateGroceryItemCommand, UpdateGroceryItemResponse>
{
    public async Task<UpdateGroceryItemResponse> Handle(UpdateGroceryItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var groceryItem = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = groceryItem ?? throw new GroceryItemNotFoundException(request.Id);
        var updatedGroceryItem = groceryItem.Update(
            request.Name, 
            request.Description, 
            request.SKU, 
            request.Barcode, 
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
        await repository.UpdateAsync(updatedGroceryItem, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("grocery item with id : {GroceryItemId} updated.", groceryItem.Id);
        return new UpdateGroceryItemResponse(groceryItem.Id);
    }
}
