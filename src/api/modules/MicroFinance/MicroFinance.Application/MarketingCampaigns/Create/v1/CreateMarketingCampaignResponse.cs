namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Create.v1;

/// <summary>
/// Response from creating a marketing campaign.
/// </summary>
public sealed record CreateMarketingCampaignResponse(DefaultIdType Id, string Name, string Code, string Status);
