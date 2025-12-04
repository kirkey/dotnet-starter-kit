using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Get.v1;

/// <summary>
/// Request to get a marketing campaign by ID.
/// </summary>
public sealed record GetMarketingCampaignRequest(Guid Id) : IRequest<MarketingCampaignResponse>;
