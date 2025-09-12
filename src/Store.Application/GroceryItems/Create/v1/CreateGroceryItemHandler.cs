namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Create.v1;

public sealed class CreateGroceryItemHandler(
    ILogger<CreateGroceryItemHandler> logger,
    [FromKeyedServices("store:grocery-items")] IRepository<GroceryItem> repository)
    : IRequestHandler<CreateGroceryItemCommand, CreateGroceryItemResponse>
{
    public async Task<CreateGroceryItemResponse> Handle(CreateGroceryItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var groceryItem = GroceryItem.Create(
            request.Name!,
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

        await repository.AddAsync(groceryItem, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("grocery item created {GroceryItemId}", groceryItem.Id);
        return new CreateGroceryItemResponse(groceryItem.Id);
    }
}
