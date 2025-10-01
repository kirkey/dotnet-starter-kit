namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Specs;

public class GroceryItemBySkuSpec : Specification<GroceryItem>
{
    public GroceryItemBySkuSpec(string sku)
    {
        Query.Where(g => g.Sku == sku);
    }
}

