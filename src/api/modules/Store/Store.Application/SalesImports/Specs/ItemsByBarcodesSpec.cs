namespace FSH.Starter.WebApi.Store.Application.SalesImports.Specs;

/// <summary>
/// Specification to find items by multiple barcodes.
/// </summary>
public class ItemsByBarcodesSpec : Specification<Item>
{
    public ItemsByBarcodesSpec(List<string> barcodes)
    {
        Query.Where(x => barcodes.Contains(x.Barcode.ToLowerInvariant()));
    }
}

