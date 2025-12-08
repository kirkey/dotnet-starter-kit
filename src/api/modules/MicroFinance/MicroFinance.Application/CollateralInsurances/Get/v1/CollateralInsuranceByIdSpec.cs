using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Get.v1;

public sealed class CollateralInsuranceByIdSpec : Specification<CollateralInsurance>, ISingleResultSpecification<CollateralInsurance>
{
    public CollateralInsuranceByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}
