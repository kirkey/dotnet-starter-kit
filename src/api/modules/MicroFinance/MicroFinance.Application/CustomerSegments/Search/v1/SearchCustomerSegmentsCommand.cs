using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Search.v1;

public class SearchCustomerSegmentsCommand : PaginationFilter, IRequest<PagedList<CustomerSegmentSummaryResponse>>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? SegmentType { get; set; }
    public string? Status { get; set; }
    public string? RiskLevel { get; set; }
    public decimal? MinIncomeLevel { get; set; }
    public decimal? MaxIncomeLevel { get; set; }
    public int? MinPriority { get; set; }
    public int? MaxPriority { get; set; }
}

public sealed record CustomerSegmentSummaryResponse(
    DefaultIdType Id,
    string Name,
    string Code,
    string SegmentType,
    string Status,
    int Priority,
    int MemberCount,
    decimal? MinIncomeLevel,
    decimal? MaxIncomeLevel,
    string? RiskLevel,
    decimal? DefaultInterestModifier,
    decimal? DefaultFeeModifier,
    string? Color,
    int DisplayOrder
);
