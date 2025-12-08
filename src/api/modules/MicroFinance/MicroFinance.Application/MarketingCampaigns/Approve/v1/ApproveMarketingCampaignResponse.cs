namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Approve.v1;

/// <summary>
/// Response from approving a marketing campaign.
/// </summary>
public sealed record ApproveMarketingCampaignResponse(DefaultIdType Id, string Status, DateOnly? ApprovedDate);
