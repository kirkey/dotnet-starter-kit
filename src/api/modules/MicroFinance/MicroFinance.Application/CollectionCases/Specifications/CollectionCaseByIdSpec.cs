using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Specifications;

public sealed class CollectionCaseByIdSpec : Specification<CollectionCase>, ISingleResultSpecification<CollectionCase>
{
    public CollectionCaseByIdSpec(DefaultIdType id)
    {
        Query.Where(c => c.Id == id);
    }
}
