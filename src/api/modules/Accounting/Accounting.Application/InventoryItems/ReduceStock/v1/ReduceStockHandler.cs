namespace Accounting.Application.InventoryItems.ReduceStock.v1;

public sealed class ReduceStockHandler(IRepository<InventoryItem> repository, ILogger<ReduceStockHandler> logger)
    : IRequestHandler<ReduceStockCommand, DefaultIdType>
{
    private readonly IRepository<InventoryItem> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<ReduceStockHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(ReduceStockCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Reducing stock from inventory item {Id}: {Amount}", request.Id, request.Amount);

        var item = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (item == null) throw new NotFoundException($"Inventory item with ID {request.Id} not found");

        item.ReduceStock(request.Amount);
        await _repository.UpdateAsync(item, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Stock reduced from {Sku}: New quantity={Quantity}", item.Sku, item.Quantity);
        return item.Id;
    }
}

