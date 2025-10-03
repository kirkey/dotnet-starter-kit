namespace FSH.Starter.WebApi.Store.Application.Items.Specs;

public class ItemByBarcodeSpec : Specification<Item>
{
    public ItemByBarcodeSpec(string barcode)
    {
        Query.Where(i => i.Barcode == barcode);
    }
}
