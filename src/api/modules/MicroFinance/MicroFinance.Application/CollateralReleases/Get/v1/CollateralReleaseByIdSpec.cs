using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Get.v1;

public sealed class CollateralReleaseByIdSpec : Specification<CollateralRelease>, ISingleResultSpecification<CollateralRelease>
{
    public CollateralReleaseByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
