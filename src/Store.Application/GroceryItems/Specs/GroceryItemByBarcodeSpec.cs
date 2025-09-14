namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Specs;

public class GroceryItemByBarcodeSpec : Specification<GroceryItem>
{
    public GroceryItemByBarcodeSpec(string barcode)
    {
        Query.Where(g => g.Barcode == barcode);
    }
}

