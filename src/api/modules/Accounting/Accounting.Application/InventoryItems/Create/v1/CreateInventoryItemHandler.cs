namespace Accounting.Application.InventoryItems.Create.v1;

public sealed class CreateInventoryItemHandler(
    IRepository<InventoryItem> repository,
    ILogger<CreateInventoryItemHandler> logger)
    : IRequestHandler<CreateInventoryItemCommand, DefaultIdType>
{
    private readonly IRepository<InventoryItem> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<CreateInventoryItemHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(CreateInventoryItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Creating inventory item {Sku}", request.Sku);

        var item = InventoryItem.Create(request.Sku, request.Name, request.Quantity, request.UnitPrice, request.Description);
        
        await _repository.AddAsync(item, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Inventory item created: {Sku} with ID {Id}", item.Sku, item.Id);
        return item.Id;
    }
}

