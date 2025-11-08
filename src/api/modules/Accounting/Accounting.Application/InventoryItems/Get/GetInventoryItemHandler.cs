using Accounting.Application.InventoryItems.Responses;

namespace Accounting.Application.InventoryItems.Get;

public sealed class GetInventoryItemHandler(
    IReadRepository<InventoryItem> repository,
    ILogger<GetInventoryItemHandler> logger)
    : IRequestHandler<GetInventoryItemRequest, InventoryItemResponse>
{
    private readonly IReadRepository<InventoryItem> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<GetInventoryItemHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<InventoryItemResponse> Handle(GetInventoryItemRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Getting inventory item {Id}", request.Id);

        var item = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (item == null) throw new InventoryItemNotFoundException(request.Id);

        return new InventoryItemResponse
        {
            Id = item.Id,
            Sku = item.Sku,
            Name = item.Name,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            Description = item.Description,
            IsActive = item.IsActive
        };
    }
}

