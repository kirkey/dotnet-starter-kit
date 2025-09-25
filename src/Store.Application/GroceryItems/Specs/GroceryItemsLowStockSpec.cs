namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Specs;

/// <summary>
/// Specification to filter grocery items that are at or below their reorder point (low stock).
/// Used for inventory management and restocking alerts.
/// </summary>
public sealed class GroceryItemsLowStockSpec : Specification<GroceryItem>
{
    public GroceryItemsLowStockSpec()
    {
        Query.Where(item => item.CurrentStock <= item.ReorderPoint);
    }
}
