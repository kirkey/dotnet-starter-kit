using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.Search.v1;

public class SearchLoanOfficerTargetsCommand : PaginationFilter, IRequest<PagedList<LoanOfficerTargetSummaryResponse>>
{
    public DefaultIdType? StaffId { get; set; }
    public string? TargetType { get; set; }
    public string? Period { get; set; }
    public DateOnly? PeriodStartFrom { get; set; }
    public DateOnly? PeriodStartTo { get; set; }
    public DateOnly? PeriodEndFrom { get; set; }
    public DateOnly? PeriodEndTo { get; set; }
    public decimal? TargetValueMin { get; set; }
    public decimal? TargetValueMax { get; set; }
}

public sealed record LoanOfficerTargetSummaryResponse(
    DefaultIdType Id,
    DefaultIdType StaffId,
    string TargetType,
    string Period,
    DateOnly PeriodStart,
    DateOnly PeriodEnd,
    decimal TargetValue,
    decimal AchievedValue,
    string? MetricUnit);
