namespace FSH.Starter.WebApi.Store.Application.Items.Specs;

public class ItemBySkuSpec : Specification<Item>
{
    public ItemBySkuSpec(string sku)
    {
        Query.Where(i => i.Sku == sku);
    }
}
