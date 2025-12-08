using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Search.v1;

public class SearchRiskIndicatorsCommand : PaginationFilter, IRequest<PagedList<RiskIndicatorSummaryResponse>>
{
    public DefaultIdType? RiskCategoryId { get; set; }
    public string? Code { get; set; }
    public string? Direction { get; set; }
    public string? Frequency { get; set; }
    public string? Status { get; set; }
    public string? Health { get; set; }
    public decimal? TargetValueMin { get; set; }
    public decimal? TargetValueMax { get; set; }
}

public sealed record RiskIndicatorSummaryResponse(
    DefaultIdType Id,
    DefaultIdType RiskCategoryId,
    string Code,
    string? Formula,
    string? Unit,
    string Direction,
    string Frequency,
    decimal? TargetValue,
    string? DataSource);
