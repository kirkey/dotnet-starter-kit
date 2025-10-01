using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Specs;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Search.v1;

public sealed class SearchInventoryTransfersHandler(
    ILogger<SearchInventoryTransfersHandler> logger,
    [FromKeyedServices("store:inventory-transfers")] IRepository<InventoryTransfer> repository)
    : IRequestHandler<SearchInventoryTransfersCommand, PagedList<GetInventoryTransferListResponse>>
{
    public async Task<PagedList<GetInventoryTransferListResponse>> Handle(SearchInventoryTransfersCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Searching inventory transfers - Page {Page} Size {Size} Term {Term}", request.PageNumber, request.PageSize, request.SearchTerm ?? string.Empty);
        
        var spec = new SearchInventoryTransfersSpecs(request);
        var paged = await repository.PaginatedListAsync(spec, new PaginationFilter { PageNumber = request.PageNumber, PageSize = request.PageSize }, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Search complete: retrieved {Count} inventory transfers", paged.TotalCount);
        return paged;
    }
}
