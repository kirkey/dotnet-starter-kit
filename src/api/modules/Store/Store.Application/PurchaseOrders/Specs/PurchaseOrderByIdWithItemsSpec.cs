namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Specs;

/// <summary>
/// Specification to retrieve a purchase order by ID with its items and related data.
/// </summary>
public sealed class PurchaseOrderByIdWithItemsSpec : Specification<PurchaseOrder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PurchaseOrderByIdWithItemsSpec"/> class.
    /// </summary>
    /// <param name="id">The purchase order ID.</param>
    public PurchaseOrderByIdWithItemsSpec(DefaultIdType id)
    {
        Query
            .Where(po => po.Id == id)
            .Include(po => po.Supplier)
            .Include(po => po.Items)
                .ThenInclude(item => item.Item);
    }
}

