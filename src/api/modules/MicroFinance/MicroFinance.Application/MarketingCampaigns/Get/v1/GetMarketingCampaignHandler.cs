using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Get.v1;

/// <summary>
/// Handler to get a marketing campaign by ID.
/// </summary>
public sealed class GetMarketingCampaignHandler(
    ILogger<GetMarketingCampaignHandler> logger,
    [FromKeyedServices("microfinance:marketingcampaigns")] IReadRepository<MarketingCampaign> repository)
    : IRequestHandler<GetMarketingCampaignRequest, MarketingCampaignResponse>
{
    public async Task<MarketingCampaignResponse> Handle(GetMarketingCampaignRequest request, CancellationToken cancellationToken)
    {
        var campaign = await repository.FirstOrDefaultAsync(new MarketingCampaignByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Marketing campaign {request.Id} not found");

        logger.LogInformation("Retrieved marketing campaign {Id}", campaign.Id);

        return new MarketingCampaignResponse(
            campaign.Id,
            campaign.Name,
            campaign.Code,
            campaign.Description,
            campaign.CampaignType,
            campaign.Status,
            campaign.StartDate,
            campaign.EndDate,
            campaign.Channels,
            campaign.Budget,
            campaign.SpentAmount,
            campaign.TargetAudience,
            campaign.ReachedCount,
            campaign.ResponseCount,
            campaign.ConversionCount,
            campaign.ResponseRate,
            campaign.ConversionRate,
            campaign.Roi);
    }
}
