using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Complete.v1;

/// <summary>
/// Handler to complete a marketing campaign.
/// </summary>
public sealed class CompleteMarketingCampaignHandler(
    ILogger<CompleteMarketingCampaignHandler> logger,
    [FromKeyedServices("microfinance:marketingcampaigns")] IRepository<MarketingCampaign> repository)
    : IRequestHandler<CompleteMarketingCampaignCommand, CompleteMarketingCampaignResponse>
{
    public async Task<CompleteMarketingCampaignResponse> Handle(CompleteMarketingCampaignCommand request, CancellationToken cancellationToken)
    {
        var campaign = await repository.FirstOrDefaultAsync(new MarketingCampaignByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Marketing campaign {request.Id} not found");

        campaign.Complete();
        await repository.UpdateAsync(campaign, cancellationToken);

        logger.LogInformation("Marketing campaign {Id} completed with {Conversions} conversions", campaign.Id, campaign.ConversionCount);

        return new CompleteMarketingCampaignResponse(
            campaign.Id,
            campaign.Status,
            campaign.ConversionCount,
            campaign.ConversionRate,
            campaign.Roi);
    }
}
