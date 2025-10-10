using FSH.Starter.WebApi.Store.Application.Items.Export.v1;

namespace FSH.Starter.WebApi.Store.Application.Items.Specs;

/// <summary>
/// Specification for filtering items during export.
/// Applies various filter criteria to build a filtered query.
/// </summary>
public sealed class ExportItemsSpec : Specification<Item>
{
    public ExportItemsSpec(ItemExportFilter? filter)
    {
        // Include related entities for export
        Query.Include(i => i.Category)
             .Include(i => i.Supplier);

        // Apply filters if provided
        if (filter == null)
        {
            return;
        }

        // Search term filter (name, SKU, or barcode)
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTerm = filter.SearchTerm.Trim();
            Query.Where(i => i.Name.Contains(searchTerm) ||
                           i.Sku.Contains(searchTerm) ||
                           i.Barcode.Contains(searchTerm) ||
                           (i.Description != null && i.Description.Contains(searchTerm)));
        }

        // Category filter
        if (filter.CategoryId.HasValue)
        {
            Query.Where(i => i.CategoryId == filter.CategoryId.Value);
        }

        // Supplier filter
        if (filter.SupplierId.HasValue)
        {
            Query.Where(i => i.SupplierId == filter.SupplierId.Value);
        }

        // Perishable filter
        if (filter.IsPerishable.HasValue)
        {
            Query.Where(i => i.IsPerishable == filter.IsPerishable.Value);
        }

        // Price range filters
        if (filter.MinPrice.HasValue)
        {
            Query.Where(i => i.UnitPrice >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            Query.Where(i => i.UnitPrice <= filter.MaxPrice.Value);
        }
        
        // Order by name for consistent export
        Query.OrderBy(i => i.Name);
    }
}

