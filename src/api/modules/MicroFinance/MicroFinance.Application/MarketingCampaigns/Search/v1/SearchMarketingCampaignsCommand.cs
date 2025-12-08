using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Search.v1;

public class SearchMarketingCampaignsCommand : PaginationFilter, IRequest<PagedList<MarketingCampaignSummaryResponse>>
{
    public string? Code { get; set; }
    public string? CampaignType { get; set; }
    public string? Status { get; set; }
    public DateOnly? StartDateFrom { get; set; }
    public DateOnly? StartDateTo { get; set; }
    public DateOnly? EndDateFrom { get; set; }
    public DateOnly? EndDateTo { get; set; }
    public string? Channels { get; set; }
    public decimal? MinBudget { get; set; }
    public decimal? MaxBudget { get; set; }
    public DefaultIdType? CreatedById { get; set; }
    public DefaultIdType? ApprovedById { get; set; }
}

public sealed record MarketingCampaignSummaryResponse(
    DefaultIdType Id,
    string Name,
    string Code,
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
    decimal? Roi
);
