using FSH.Starter.WebApi.Store.Application.GroceryItems.Export.v1;

namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Specs;

/// <summary>
/// Specification for filtering grocery items for export based on various criteria.
/// Uses the same pattern as SearchGroceryItemsSpecs but for export functionality.
/// </summary>
public class ExportGroceryItemsSpec : Specification<GroceryItem>
{
    public ExportGroceryItemsSpec(ExportGroceryItemsQuery request)
    {
        Query.OrderBy(g => g.Name);
        
        // Category filter
        if (request.CategoryId.HasValue)
        {
            Query.Where(g => g.CategoryId == request.CategoryId.Value);
        }
        
        // Supplier filter
        if (request.SupplierId.HasValue)
        {
            Query.Where(g => g.SupplierId == request.SupplierId.Value);
        }
        
        // Search term filter (name, SKU, or barcode)
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            Query.Where(g => g.Name!.Contains(request.SearchTerm) || 
                           g.Sku.Contains(request.SearchTerm) || 
                           g.Barcode.Contains(request.SearchTerm));
        }
        
        // Low-stock filter
        if (request.OnlyLowStock)
        {
            Query.Where(g => g.CurrentStock <= g.ReorderPoint);
        }
        
        // Perishable items filter
        if (request.OnlyPerishable)
        {
            Query.Where(g => g.IsPerishable == true);
        }
        
        // Active/inactive filter (default to active only)
        if (!request.IncludeInactive)
        {
            // Note: Assuming GroceryItem has IsActive property or equivalent
            // If not available, this condition can be removed
        }
    }
}
