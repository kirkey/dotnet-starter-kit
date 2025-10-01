namespace FSH.Starter.WebApi.Store.Application.PriceLists.Specs;

public class GetPriceListSpecification : Specification<PriceList>
{
    public GetPriceListSpecification(DefaultIdType id)
    {
        Query.Where(p => p.Id == id);
    }
}

