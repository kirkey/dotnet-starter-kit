namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Specs;

/// <summary>
/// Specification to filter grocery items by category ID.
/// Used for category-based filtering and reporting.
/// </summary>
public sealed class GroceryItemsByCategoryIdSpec : Specification<GroceryItem>
{
    public GroceryItemsByCategoryIdSpec(DefaultIdType categoryId)
    {
        Query.Where(item => item.CategoryId == categoryId);
    }
}
