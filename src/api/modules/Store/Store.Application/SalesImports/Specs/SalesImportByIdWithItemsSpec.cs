namespace FSH.Starter.WebApi.Store.Application.SalesImports.Specs;

/// <summary>
/// Specification to get a sales import by ID with all related items.
/// </summary>
public class SalesImportByIdWithItemsSpec : Specification<SalesImport>
{
    public SalesImportByIdWithItemsSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Warehouse)
            .Include(x => x.Items)
                .ThenInclude(i => i.Item)
            .Include(x => x.Items)
                .ThenInclude(i => i.InventoryTransaction);
    }
}

