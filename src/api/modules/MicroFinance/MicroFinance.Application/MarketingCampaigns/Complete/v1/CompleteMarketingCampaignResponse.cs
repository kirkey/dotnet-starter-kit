namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Complete.v1;

/// <summary>
/// Response from completing a marketing campaign.
/// </summary>
public sealed record CompleteMarketingCampaignResponse(
    Guid Id,
    string Status,
    int ConversionCount,
    decimal? ConversionRate,
    decimal? Roi);
