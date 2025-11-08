
namespace Accounting.Application.InventoryItems.Update.v1;

public sealed class UpdateInventoryItemHandler(
    IRepository<InventoryItem> repository,
    ILogger<UpdateInventoryItemHandler> logger)
    : IRequestHandler<UpdateInventoryItemCommand, DefaultIdType>
{
    private readonly IRepository<InventoryItem> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<UpdateInventoryItemHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(UpdateInventoryItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Updating inventory item {Id}", request.Id);

        var item = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (item == null) throw new InventoryItemNotFoundException(request.Id);

        item.Update(request.Sku, request.Name, request.Quantity, request.UnitPrice, request.Description);
        await _repository.UpdateAsync(item, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Inventory item {Sku} updated successfully", item.Sku);
        return item.Id;
    }
}

