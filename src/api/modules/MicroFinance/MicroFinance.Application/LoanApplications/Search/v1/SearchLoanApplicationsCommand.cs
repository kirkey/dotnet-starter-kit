using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Search.v1;

public class SearchLoanApplicationsCommand : PaginationFilter, IRequest<PagedList<LoanApplicationSummaryResponse>>
{
    public DefaultIdType? MemberId { get; set; }
    public DefaultIdType? LoanProductId { get; set; }
    public string? ApplicationNumber { get; set; }
    public string? Status { get; set; }
    public DateOnly? ApplicationDateFrom { get; set; }
    public DateOnly? ApplicationDateTo { get; set; }
    public decimal? MinRequestedAmount { get; set; }
    public decimal? MaxRequestedAmount { get; set; }
    public DefaultIdType? AssignedOfficerId { get; set; }
}

public sealed record LoanApplicationSummaryResponse(
    DefaultIdType Id,
    string ApplicationNumber,
    DefaultIdType MemberId,
    DefaultIdType LoanProductId,
    DateOnly ApplicationDate,
    decimal RequestedAmount,
    decimal? ApprovedAmount,
    int RequestedTermMonths,
    string? Purpose,
    string Status,
    DefaultIdType? AssignedOfficerId,
    DateTime? DecisionAt
);
