using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Launch.v1;

/// <summary>
/// Command to launch a marketing campaign.
/// </summary>
public sealed record LaunchMarketingCampaignCommand(Guid Id) : IRequest<LaunchMarketingCampaignResponse>;
