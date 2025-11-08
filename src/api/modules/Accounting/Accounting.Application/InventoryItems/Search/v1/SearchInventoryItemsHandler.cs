using Accounting.Application.InventoryItems.Responses;

namespace Accounting.Application.InventoryItems.Search.v1;

public sealed class SearchInventoryItemsHandler(
    IReadRepository<InventoryItem> repository,
    ILogger<SearchInventoryItemsHandler> logger)
    : IRequestHandler<SearchInventoryItemsRequest, PagedList<InventoryItemResponse>>
{
    private readonly IReadRepository<InventoryItem> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<SearchInventoryItemsHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<PagedList<InventoryItemResponse>> Handle(SearchInventoryItemsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Searching inventory items");

        var spec = new SearchInventoryItemsSpec(request);
        var items = await _repository.ListAsync(spec, cancellationToken);
        var totalCount = await _repository.CountAsync(cancellationToken);

        var response = items.Select(item => new InventoryItemResponse
        {
            Id = item.Id,
            Sku = item.Sku,
            Name = item.Name,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            Description = item.Description,
            IsActive = item.IsActive
        }).ToList();

        return new PagedList<InventoryItemResponse>(response, totalCount, request.PageNumber, request.PageSize);
    }
}

