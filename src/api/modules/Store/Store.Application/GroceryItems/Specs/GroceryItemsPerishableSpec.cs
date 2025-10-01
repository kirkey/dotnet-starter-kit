namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Specs;

/// <summary>
/// Specification to filter only perishable grocery items.
/// Used for expiry tracking and rotation management (FIFO).
/// </summary>
public sealed class GroceryItemsPerishableSpec : Specification<GroceryItem>
{
    public GroceryItemsPerishableSpec()
    {
        Query.Where(item => item.IsPerishable == true);
    }
}
