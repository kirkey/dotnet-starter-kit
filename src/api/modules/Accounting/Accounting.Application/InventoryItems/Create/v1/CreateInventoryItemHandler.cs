namespace Accounting.Application.InventoryItems.Create.v1;

public sealed class CreateInventoryItemHandler(
    [FromKeyedServices("accounting:inventory-items")] IRepository<InventoryItem> repository,
    ILogger<CreateInventoryItemHandler> logger)
    : IRequestHandler<CreateInventoryItemCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateInventoryItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        logger.LogInformation("Creating inventory item {Sku}", request.Sku);

        var item = InventoryItem.Create(request.Sku, request.Name, request.Quantity, request.UnitPrice, request.Description, request.ImageUrl);
        
        await repository.AddAsync(item, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Inventory item created: {Sku} with ID {Id}", item.Sku, item.Id);
        return item.Id;
    }
}

