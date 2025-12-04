namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Launch.v1;

/// <summary>
/// Response from launching a marketing campaign.
/// </summary>
public sealed record LaunchMarketingCampaignResponse(Guid Id, string Status);
