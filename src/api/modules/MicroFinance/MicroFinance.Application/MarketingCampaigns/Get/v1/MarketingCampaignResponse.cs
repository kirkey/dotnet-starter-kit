namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Get.v1;

/// <summary>
/// Response containing marketing campaign details.
/// </summary>
public sealed record MarketingCampaignResponse(
    DefaultIdType Id,
    string Name,
    string Code,
    string? Description,
    string CampaignType,
    string Status,
    DateOnly StartDate,
    DateOnly? EndDate,
    string Channels,
    decimal Budget,
    decimal SpentAmount,
    int TargetAudience,
    int ReachedCount,
    int ResponseCount,
    int ConversionCount,
    decimal? ResponseRate,
    decimal? ConversionRate,
    decimal? Roi);
