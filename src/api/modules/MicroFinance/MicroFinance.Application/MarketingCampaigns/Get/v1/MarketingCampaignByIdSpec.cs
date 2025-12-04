using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Get.v1;

/// <summary>
/// Specification to get a marketing campaign by ID.
/// </summary>
public sealed class MarketingCampaignByIdSpec : Specification<MarketingCampaign>, ISingleResultSpecification<MarketingCampaign>
{
    public MarketingCampaignByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
