using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Specs;

public class SearchPurchaseOrdersSpecs : Specification<PurchaseOrder, GetPurchaseOrderListResponse>
{
    public SearchPurchaseOrdersSpecs(SearchPurchaseOrdersCommand request)
    {
        Query.Include(po => po.Supplier);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm!;
            Query.Where(po =>
                po.OrderNumber.Contains(term) ||
                (po.Supplier != null && po.Supplier.Name!.Contains(term)) ||
                (po.Notes != null && po.Notes.Contains(term))
            );
        }

        if (request.SupplierId.HasValue)
        {
            var supplierId = request.SupplierId.Value;
            Query.Where(po => po.SupplierId == supplierId);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            var status = request.Status!;
            Query.Where(po => po.Status == status);
        }

        if (request.FromDate.HasValue)
        {
            var from = request.FromDate.Value;
            Query.Where(po => po.OrderDate >= from);
        }

        if (request.ToDate.HasValue)
        {
            var to = request.ToDate.Value;
            Query.Where(po => po.OrderDate <= to);
        }

        Query.Select(po => new GetPurchaseOrderListResponse(
            po.Id,
            po.OrderNumber,
            po.SupplierId,
            po.OrderDate,
            po.Status,
            po.TotalAmount));

        Query.OrderByDescending(po => po.OrderDate).ThenBy(po => po.OrderNumber);
    }
}
