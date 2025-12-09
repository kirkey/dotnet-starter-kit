using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Search.v1;

public class SearchInterestRateChangesCommand : PaginationFilter, IRequest<PagedList<InterestRateChangeSummaryResponse>>
{
    public DefaultIdType? LoanId { get; set; }
    public string? Reference { get; set; }
    public string? ChangeType { get; set; }
    public string? Status { get; set; }
    public DateOnly? RequestDateFrom { get; set; }
    public DateOnly? RequestDateTo { get; set; }
    public DateOnly? EffectiveDateFrom { get; set; }
    public DateOnly? EffectiveDateTo { get; set; }
}

public sealed record InterestRateChangeSummaryResponse(
    DefaultIdType Id,
    DefaultIdType LoanId,
    string Reference,
    string ChangeType,
    DateOnly RequestDate,
    DateOnly EffectiveDate,
    decimal PreviousRate,
    decimal NewRate,
    decimal RateChange,
    string ChangeReason,
    string Status,
    string? ApprovedBy,
    DateOnly? ApprovalDate);
