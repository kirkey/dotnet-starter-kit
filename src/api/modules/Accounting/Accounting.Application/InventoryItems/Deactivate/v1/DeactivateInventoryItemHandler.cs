namespace Accounting.Application.InventoryItems.Deactivate.v1;

public sealed class DeactivateInventoryItemHandler(
    IRepository<InventoryItem> repository,
    ILogger<DeactivateInventoryItemHandler> logger)
    : IRequestHandler<DeactivateInventoryItemCommand, DefaultIdType>
{
    private readonly IRepository<InventoryItem> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<DeactivateInventoryItemHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(DeactivateInventoryItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Deactivating inventory item {Id}", request.Id);

        var item = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (item == null) throw new NotFoundException($"Inventory item with ID {request.Id} not found");

        item.Deactivate();
        await _repository.UpdateAsync(item, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Inventory item {Sku} deactivated successfully", item.Sku);
        return item.Id;
    }
}
