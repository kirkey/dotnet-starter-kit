using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Create.v1;

/// <summary>
/// Command to create a new marketing campaign.
/// </summary>
public sealed record CreateMarketingCampaignCommand(
    string Name,
    string Code,
    string CampaignType,
    DateOnly StartDate,
    string Channels,
    decimal Budget,
    int TargetAudience,
    string? Description = null,
    DateOnly? EndDate = null) : IRequest<CreateMarketingCampaignResponse>;
