using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Approve.v1;

/// <summary>
/// Command to approve a marketing campaign.
/// </summary>
public sealed record ApproveMarketingCampaignCommand(Guid Id, Guid ApprovedById) : IRequest<ApproveMarketingCampaignResponse>;
