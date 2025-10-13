namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Search.v1;

/// <summary>
/// Command for searching purchase orders with pagination and filtering support.
/// </summary>
public class SearchPurchaseOrdersCommand : PaginationFilter, IRequest<PagedList<GetPurchaseOrderListResponse>>
{
    /// <summary>
    /// Search term to filter by order number, supplier name, or notes.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Filter by supplier ID.
    /// </summary>
    public DefaultIdType? SupplierId { get; set; }

    /// <summary>
    /// Filter by purchase order status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by order date from this date onwards.
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// Filter by order date up to this date.
    /// </summary>
    public DateTime? ToDate { get; set; }
}

