using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Search.v1;

public class SearchRiskAlertsCommand : PaginationFilter, IRequest<PagedList<RiskAlertSummaryResponse>>
{
    public string? AlertNumber { get; set; }
    public DefaultIdType? RiskCategoryId { get; set; }
    public DefaultIdType? RiskIndicatorId { get; set; }
    public string? Severity { get; set; }
    public string? Source { get; set; }
    public string? Status { get; set; }
    public DefaultIdType? BranchId { get; set; }
    public DefaultIdType? LoanId { get; set; }
    public DefaultIdType? MemberId { get; set; }
    public DefaultIdType? AssignedToUserId { get; set; }
    public DateTime? AlertedFrom { get; set; }
    public DateTime? AlertedTo { get; set; }
}

public sealed record RiskAlertSummaryResponse(
    DefaultIdType Id,
    string AlertNumber,
    string Title,
    DefaultIdType? RiskCategoryId,
    DefaultIdType? RiskIndicatorId,
    string Severity,
    string Source,
    string Status,
    decimal? ThresholdValue,
    decimal? ActualValue,
    DateTime AlertedAt,
    DefaultIdType? BranchId,
    DefaultIdType? LoanId,
    DefaultIdType? MemberId,
    DefaultIdType? AssignedToUserId,
    DateTime? ResolvedAt
);
