using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Create.v1;

/// <summary>
/// Handler for creating a new marketing campaign.
/// </summary>
public sealed class CreateMarketingCampaignHandler(
    ILogger<CreateMarketingCampaignHandler> logger,
    [FromKeyedServices("microfinance:marketingcampaigns")] IRepository<MarketingCampaign> repository)
    : IRequestHandler<CreateMarketingCampaignCommand, CreateMarketingCampaignResponse>
{
    public async Task<CreateMarketingCampaignResponse> Handle(CreateMarketingCampaignCommand request, CancellationToken cancellationToken)
    {
        var campaign = MarketingCampaign.Create(
            request.Name,
            request.Code,
            request.CampaignType,
            request.StartDate,
            request.Channels,
            request.Budget,
            request.TargetAudience,
            request.Description,
            request.EndDate);

        await repository.AddAsync(campaign, cancellationToken);
        logger.LogInformation("Marketing campaign '{Name}' created with ID {Id}", campaign.Name, campaign.Id);

        return new CreateMarketingCampaignResponse(campaign.Id, campaign.Name, campaign.Code, campaign.Status);
    }
}
