

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Search.v1;

public class SearchInventoryTransfersSpecs : Specification<InventoryTransfer>
{
    public SearchInventoryTransfersSpecs(SearchInventoryTransfersCommand request)
    {
        Query.Include(it => it.FromWarehouse);
        Query.Include(it => it.ToWarehouse);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            Query.Where(it => it.Name!.Contains(request.SearchTerm) ||
                            it.TransferNumber.Contains(request.SearchTerm) ||
                            it.Reason.Contains(request.SearchTerm));
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

        Query.OrderByDescending(it => it.TransferDate).ThenBy(it => it.TransferNumber);
    }
}
