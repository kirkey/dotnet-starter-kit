using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Launch.v1;

/// <summary>
/// Handler to launch a marketing campaign.
/// </summary>
public sealed class LaunchMarketingCampaignHandler(
    ILogger<LaunchMarketingCampaignHandler> logger,
    [FromKeyedServices("microfinance:marketingcampaigns")] IRepository<MarketingCampaign> repository)
    : IRequestHandler<LaunchMarketingCampaignCommand, LaunchMarketingCampaignResponse>
{
    public async Task<LaunchMarketingCampaignResponse> Handle(LaunchMarketingCampaignCommand request, CancellationToken cancellationToken)
    {
        var campaign = await repository.FirstOrDefaultAsync(new MarketingCampaignByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Marketing campaign {request.Id} not found");

        campaign.Launch();
        await repository.UpdateAsync(campaign, cancellationToken);

        logger.LogInformation("Marketing campaign {Id} launched", campaign.Id);

        return new LaunchMarketingCampaignResponse(campaign.Id, campaign.Status);
    }
}
