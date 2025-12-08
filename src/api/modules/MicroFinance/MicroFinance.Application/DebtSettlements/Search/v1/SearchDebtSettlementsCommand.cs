using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Search.v1;

public class SearchDebtSettlementsCommand : PaginationFilter, IRequest<PagedList<DebtSettlementSummaryResponse>>
{
    public string? ReferenceNumber { get; set; }
    public DefaultIdType? CollectionCaseId { get; set; }
    public DefaultIdType? LoanId { get; set; }
    public DefaultIdType? MemberId { get; set; }
    public string? SettlementType { get; set; }
    public string? Status { get; set; }
    public DateOnly? ProposedDateFrom { get; set; }
    public DateOnly? ProposedDateTo { get; set; }
    public DateOnly? DueDateFrom { get; set; }
    public DateOnly? DueDateTo { get; set; }
}

public sealed record DebtSettlementSummaryResponse(
    DefaultIdType Id,
    string ReferenceNumber,
    DefaultIdType CollectionCaseId,
    DefaultIdType LoanId,
    DefaultIdType MemberId,
    string SettlementType,
    string Status,
    decimal OriginalOutstanding,
    decimal SettlementAmount,
    decimal DiscountAmount,
    decimal DiscountPercentage,
    decimal AmountPaid,
    decimal RemainingBalance,
    DateOnly ProposedDate,
    DateOnly DueDate,
    DateOnly? CompletedDate
);
