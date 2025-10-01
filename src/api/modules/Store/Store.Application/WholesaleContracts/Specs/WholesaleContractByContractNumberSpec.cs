namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Specs;

public sealed class WholesaleContractByContractNumberSpec : Specification<WholesaleContract>, ISingleResultSpecification<WholesaleContract>
{
    public WholesaleContractByContractNumberSpec(string contractNumber)
    {
        Query.Where(x => x.ContractNumber == contractNumber);
    }
}
