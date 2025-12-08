using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Get.v1;

public sealed class RiskCategoryByIdSpec : Specification<RiskCategory>, ISingleResultSpecification<RiskCategory>
{
    public RiskCategoryByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}
