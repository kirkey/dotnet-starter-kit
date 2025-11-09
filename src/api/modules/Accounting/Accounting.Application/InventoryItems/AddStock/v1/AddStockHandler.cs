
namespace Accounting.Application.InventoryItems.AddStock.v1;

public sealed class AddStockHandler(IRepository<InventoryItem> repository, ILogger<AddStockHandler> logger)
    : IRequestHandler<AddStockCommand, DefaultIdType>
{
    private readonly IRepository<InventoryItem> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<AddStockHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(AddStockCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Adding stock to inventory item {Id}: {Quantity}", request.Id, request.Quantity);

        var item = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (item == null) throw new InventoryItemNotFoundException(request.Id);

        item.AddStock(request.Quantity);
        await _repository.UpdateAsync(item, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Stock added to {Sku}: New quantity={Quantity}", item.Sku, item.Quantity);
        return item.Id;
    }
}

