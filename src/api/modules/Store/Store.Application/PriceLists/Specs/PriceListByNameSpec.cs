namespace FSH.Starter.WebApi.Store.Application.PriceLists.Specs;

public class PriceListByNameSpec : Specification<PriceList>
{
    public PriceListByNameSpec(string name)
    {
        Query.Where(p => p.PriceListName == name);
    }
}
