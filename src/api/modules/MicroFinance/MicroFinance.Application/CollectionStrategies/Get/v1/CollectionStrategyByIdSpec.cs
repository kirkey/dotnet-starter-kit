using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Get.v1;

public sealed class CollectionStrategyByIdSpec : Specification<CollectionStrategy>, ISingleResultSpecification<CollectionStrategy>
{
    public CollectionStrategyByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}
