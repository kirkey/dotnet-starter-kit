using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Specs;

/// <summary>
/// Specification for searching inventory transfers with various filters and pagination support.
/// </summary>
public class SearchInventoryTransfersSpecs : EntitiesByPaginationFilterSpec<InventoryTransfer, GetInventoryTransferListResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchInventoryTransfersSpecs"/> class.
    /// </summary>
    /// <param name="request">The search inventory transfers command containing filter criteria and pagination parameters.</param>
    public SearchInventoryTransfersSpecs(SearchInventoryTransfersCommand request)
        : base(request)
    {
        Query
            .Include(it => it.FromWarehouse)
            .Include(it => it.ToWarehouse)
            .Where(it =>
                it.TransferNumber.Contains(request.SearchTerm!) ||
                (it.TransportMethod != null && it.TransportMethod.Contains(request.SearchTerm!)) ||
                (it.TrackingNumber != null && it.TrackingNumber.Contains(request.SearchTerm!)) ||
                (it.RequestedBy != null && it.RequestedBy.Contains(request.SearchTerm!)),
                !string.IsNullOrWhiteSpace(request.SearchTerm))
            .Where(it => it.FromWarehouseId == request.FromWarehouseId!.Value, request.FromWarehouseId.HasValue)
            .Where(it => it.ToWarehouseId == request.ToWarehouseId!.Value, request.ToWarehouseId.HasValue)
            .Where(it => it.Status == request.Status, !string.IsNullOrWhiteSpace(request.Status))
            .Where(it => it.TransferDate >= request.FromDate!.Value, request.FromDate.HasValue)
            .Where(it => it.TransferDate <= request.ToDate!.Value, request.ToDate.HasValue)
            .OrderByDescending(it => it.TransferDate, !request.HasOrderBy())
            .ThenBy(it => it.TransferNumber);
    }
}
