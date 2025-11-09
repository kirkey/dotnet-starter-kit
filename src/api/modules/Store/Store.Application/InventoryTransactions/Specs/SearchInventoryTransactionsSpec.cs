using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Get.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Specs;

public class SearchInventoryTransactionsSpec : EntitiesByPaginationFilterSpec<InventoryTransaction, InventoryTransactionResponse>
{
    public SearchInventoryTransactionsSpec(SearchInventoryTransactionsCommand request)
        : base(request)
    {
        Query
            .OrderByDescending(t => t.TransactionDate)
            .ThenBy(t => t.TransactionNumber);

        if (!string.IsNullOrWhiteSpace(request.TransactionNumber))
        {
            Query.Where(t => t.TransactionNumber.Contains(request.TransactionNumber));
        }

        if (request.ItemId.HasValue)
        {
            Query.Where(t => t.ItemId == request.ItemId.Value);
        }

        if (request.WarehouseId.HasValue)
        {
            Query.Where(t => t.WarehouseId == request.WarehouseId.Value);
        }

        if (request.WarehouseLocationId.HasValue)
        {
            Query.Where(t => t.WarehouseLocationId == request.WarehouseLocationId.Value);
        }

        if (request.PurchaseOrderId.HasValue)
        {
            Query.Where(t => t.PurchaseOrderId == request.PurchaseOrderId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.TransactionType))
        {
            Query.Where(t => t.TransactionType == request.TransactionType);
        }

        if (!string.IsNullOrWhiteSpace(request.Reason))
        {
            Query.Where(t => t.Reason.Contains(request.Reason));
        }

        if (request.TransactionDateFrom.HasValue)
        {
            Query.Where(t => t.TransactionDate >= request.TransactionDateFrom.Value);
        }

        if (request.TransactionDateTo.HasValue)
        {
            Query.Where(t => t.TransactionDate <= request.TransactionDateTo.Value);
        }

        if (request.IsApproved.HasValue)
        {
            Query.Where(t => t.IsApproved == request.IsApproved.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.PerformedBy))
        {
            Query.Where(t => t.PerformedBy != null && t.PerformedBy.Contains(request.PerformedBy));
        }

        if (!string.IsNullOrWhiteSpace(request.ApprovedBy))
        {
            Query.Where(t => t.ApprovedBy != null && t.ApprovedBy.Contains(request.ApprovedBy));
        }

        if (request.MinTotalCost.HasValue)
        {
            Query.Where(t => t.TotalCost >= request.MinTotalCost.Value);
        }

        if (request.MaxTotalCost.HasValue)
        {
            Query.Where(t => t.TotalCost <= request.MaxTotalCost.Value);
        }
    }
}
