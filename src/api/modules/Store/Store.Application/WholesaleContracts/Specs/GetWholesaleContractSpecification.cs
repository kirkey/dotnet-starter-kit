namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Specs;

public class GetWholesaleContractSpecification : Specification<WholesaleContract>
{
    public GetWholesaleContractSpecification(DefaultIdType id)
    {
        Query.Where(p => p.Id == id);
    }
}

