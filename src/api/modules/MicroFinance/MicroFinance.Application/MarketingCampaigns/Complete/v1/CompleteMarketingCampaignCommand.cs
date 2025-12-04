using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Complete.v1;

/// <summary>
/// Command to complete a marketing campaign.
/// </summary>
public sealed record CompleteMarketingCampaignCommand(Guid Id) : IRequest<CompleteMarketingCampaignResponse>;
