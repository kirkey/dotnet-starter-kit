using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Get.v1;

/// <summary>
/// Specification to get a risk indicator by ID.
/// </summary>
public sealed class RiskIndicatorByIdSpec : Specification<RiskIndicator>, ISingleResultSpecification<RiskIndicator>
{
    public RiskIndicatorByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}
