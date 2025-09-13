using FSH.Framework.Core.Paging;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Search.v1;

public sealed class SearchInventoryTransfersHandler(
    ILogger<SearchInventoryTransfersHandler> logger,
    [FromKeyedServices("store:inventory-transfers")] IRepository<InventoryTransfer> repository)
    : IRequestHandler<SearchInventoryTransfersCommand, PagedList<GetInventoryTransferListResponse>>
{
    public async Task<PagedList<GetInventoryTransferListResponse>> Handle(SearchInventoryTransfersCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var spec = new SearchInventoryTransfersSpecs(request);
        var inventoryTransfers = await repository.PaginatedListAsync(spec, new PaginationFilter(request.PageNumber, request.PageSize), cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Retrieved {Count} inventory transfers", inventoryTransfers.Data.Count);
        
        return inventoryTransfers.Select(it => new GetInventoryTransferListResponse(
            it.Id,
            it.TransferNumber,
            it.FromWarehouseId,
            it.FromWarehouse.Name!,
            it.ToWarehouseId,
            it.ToWarehouse.Name!,
            it.TransferDate,
            it.Status,
            it.TransferType,
            it.Priority));
    }
}
