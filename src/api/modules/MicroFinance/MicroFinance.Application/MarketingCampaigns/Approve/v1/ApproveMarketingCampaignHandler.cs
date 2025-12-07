using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Approve.v1;

/// <summary>
/// Handler to approve a marketing campaign.
/// </summary>
public sealed class ApproveMarketingCampaignHandler(
    ILogger<ApproveMarketingCampaignHandler> logger,
    [FromKeyedServices("microfinance:marketingcampaigns")] IRepository<MarketingCampaign> repository)
    : IRequestHandler<ApproveMarketingCampaignCommand, ApproveMarketingCampaignResponse>
{
    public async Task<ApproveMarketingCampaignResponse> Handle(ApproveMarketingCampaignCommand request, CancellationToken cancellationToken)
    {
        var campaign = await repository.FirstOrDefaultAsync(new MarketingCampaignByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Marketing campaign {request.Id} not found");

        campaign.Approve(request.ApprovedById);
        await repository.UpdateAsync(campaign, cancellationToken);

        logger.LogInformation("Marketing campaign {Id} approved", campaign.Id);

        return new ApproveMarketingCampaignResponse(campaign.Id, campaign.Status, campaign.ApprovedDate);
    }
}
