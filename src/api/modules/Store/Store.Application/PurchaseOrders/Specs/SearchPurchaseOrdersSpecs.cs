using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Specs;

/// <summary>
/// Specification for searching purchase orders with various filters and pagination support.
/// </summary>
public class SearchPurchaseOrdersSpecs : EntitiesByPaginationFilterSpec<PurchaseOrder, GetPurchaseOrderListResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchPurchaseOrdersSpecs"/> class.
    /// </summary>
    /// <param name="request">The search purchase orders command containing filter criteria and pagination parameters.</param>
    public SearchPurchaseOrdersSpecs(SearchPurchaseOrdersCommand request)
        : base(request)
    {
        Query
            .Include(po => po.Supplier)
            .Where(po =>
                po.OrderNumber.Contains(request.SearchTerm!) ||
                (po.Supplier != null && po.Supplier.Name != null && po.Supplier.Name.Contains(request.SearchTerm!)) ||
                (po.Notes != null && po.Notes.Contains(request.SearchTerm!)),
                !string.IsNullOrWhiteSpace(request.SearchTerm))
            .Where(po => po.SupplierId == request.SupplierId!.Value, request.SupplierId.HasValue)
            .Where(po => po.Status == request.Status, !string.IsNullOrWhiteSpace(request.Status))
            .Where(po => po.OrderDate >= request.FromDate!.Value, request.FromDate.HasValue)
            .Where(po => po.OrderDate <= request.ToDate!.Value, request.ToDate.HasValue)
            .OrderByDescending(po => po.OrderDate, !request.HasOrderBy())
            .ThenBy(po => po.OrderNumber);
    }
}
