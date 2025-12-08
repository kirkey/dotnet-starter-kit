using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Search.v1;

public class SearchRiskCategoriesCommand : PaginationFilter, IRequest<PagedList<RiskCategorySummaryResponse>>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? RiskType { get; set; }
    public DefaultIdType? ParentCategoryId { get; set; }
    public string? DefaultSeverity { get; set; }
    public bool? RequiresEscalation { get; set; }
    public string? Status { get; set; }
}

public sealed record RiskCategorySummaryResponse(
    DefaultIdType Id,
    string Code,
    string Name,
    string? Description,
    string RiskType,
    DefaultIdType? ParentCategoryId,
    string DefaultSeverity,
    decimal WeightFactor,
    decimal? AlertThreshold,
    bool RequiresEscalation,
    int? EscalationHours
);
