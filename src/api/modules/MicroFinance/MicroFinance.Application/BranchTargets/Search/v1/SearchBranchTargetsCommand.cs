using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.Search.v1;

public class SearchBranchTargetsCommand : PaginationFilter, IRequest<PagedList<BranchTargetSummaryResponse>>
{
    public DefaultIdType? BranchId { get; set; }
    public string? TargetType { get; set; }
    public string? Period { get; set; }
    public string? Status { get; set; }
    public DateOnly? PeriodStartFrom { get; set; }
    public DateOnly? PeriodStartTo { get; set; }
    public DateOnly? PeriodEndFrom { get; set; }
    public DateOnly? PeriodEndTo { get; set; }
}

public sealed record BranchTargetSummaryResponse(
    DefaultIdType Id,
    DefaultIdType BranchId,
    string TargetType,
    string? Description,
    string Period,
    DateOnly PeriodStart,
    DateOnly PeriodEnd,
    decimal TargetValue,
    string? MetricUnit,
    decimal AchievedValue,
    decimal AchievementPercentage,
    string Status,
    decimal? MinimumThreshold,
    decimal? StretchTarget,
    decimal Weight
);
