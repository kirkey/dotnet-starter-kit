using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Get.v1;

public sealed class CollateralValuationByIdSpec : Specification<CollateralValuation>, ISingleResultSpecification<CollateralValuation>
{
    public CollateralValuationByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
