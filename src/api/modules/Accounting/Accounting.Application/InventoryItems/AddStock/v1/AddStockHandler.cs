namespace Accounting.Application.InventoryItems.AddStock.v1;

public sealed class AddStockHandler(IRepository<InventoryItem> repository, ILogger<AddStockHandler> logger)
    : IRequestHandler<AddStockCommand, DefaultIdType>
{
    private readonly IRepository<InventoryItem> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<AddStockHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(AddStockCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Adding stock to inventory item {Id}: {Amount}", request.Id, request.Amount);

        var item = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (item == null) throw new NotFoundException($"Inventory item with ID {request.Id} not found");

        item.AddStock(request.Amount);
        await _repository.UpdateAsync(item, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Stock added to {Sku}: New quantity={Quantity}", item.Sku, item.Quantity);
        return item.Id;
    }
}

