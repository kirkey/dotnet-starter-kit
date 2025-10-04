using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Specs;

public class SearchInventoryTransfersSpecs : Specification<InventoryTransfer, GetInventoryTransferListResponse>
{
    public SearchInventoryTransfersSpecs(SearchInventoryTransfersCommand request)
    {
        Query.Include(it => it.FromWarehouse);
        Query.Include(it => it.ToWarehouse);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            Query.Where(it =>
                it.TransferNumber.Contains(request.SearchTerm) ||
                (it.TransportMethod != null && it.TransportMethod.Contains(request.SearchTerm)) ||
                (it.TrackingNumber != null && it.TrackingNumber.Contains(request.SearchTerm)) ||
                (it.RequestedBy != null && it.RequestedBy.Contains(request.SearchTerm))
            );
        }

        if (request.FromWarehouseId.HasValue)
        {
            Query.Where(it => it.FromWarehouseId == request.FromWarehouseId.Value);
        }

        if (request.ToWarehouseId.HasValue)
        {
            Query.Where(it => it.ToWarehouseId == request.ToWarehouseId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            Query.Where(it => it.Status == request.Status);
        }

        if (request.FromDate.HasValue)
        {
            Query.Where(it => it.TransferDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            Query.Where(it => it.TransferDate <= request.ToDate.Value);
        }

        // Project to DTO expected by handlers and PaginatedListAsync
        Query.Select(it => new GetInventoryTransferListResponse(
            it.Id,
            it.Name,
            it.Description,
            it.Notes,
            it.TransferNumber,
            it.FromWarehouseId,
            it.FromWarehouse != null ? it.FromWarehouse.Name : string.Empty,
            it.ToWarehouseId,
            it.ToWarehouse != null ? it.ToWarehouse.Name : string.Empty,
            it.TransferDate,
            it.Status,
            it.TransferType,
            it.Priority));

        Query.OrderByDescending(it => it.TransferDate).ThenBy(it => it.TransferNumber);
    }
}
