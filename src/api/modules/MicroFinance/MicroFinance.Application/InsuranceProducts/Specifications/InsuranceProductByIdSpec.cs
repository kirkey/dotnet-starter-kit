using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Specifications;

public sealed class InsuranceProductByIdSpec : Specification<InsuranceProduct>, ISingleResultSpecification<InsuranceProduct>
{
    public InsuranceProductByIdSpec(DefaultIdType id)
    {
        Query.Where(p => p.Id == id);
    }
}
