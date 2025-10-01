namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Specs;

/// <summary>
/// Specification to search grocery items by a general search term.
/// Searches across Name, SKU, and Barcode fields for flexible filtering.
/// </summary>
public sealed class SearchGroceryItemsByTermSpec : Specification<GroceryItem>
{
    /// <summary>
    /// Creates a specification that searches for items matching the term in name, SKU, or barcode.
    /// </summary>
    /// <param name="searchTerm">The term to search for (case-insensitive)</param>
    public SearchGroceryItemsByTermSpec(string searchTerm)
    {
        Query.Where(item => 
            item.Name.Contains(searchTerm) ||
            item.Sku.Contains(searchTerm) ||
            item.Barcode.Contains(searchTerm));
    }
}
