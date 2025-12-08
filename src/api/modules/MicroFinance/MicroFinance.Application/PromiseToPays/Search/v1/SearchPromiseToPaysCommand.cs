using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.Search.v1;

public class SearchPromiseToPaysCommand : PaginationFilter, IRequest<PagedList<PromiseToPaySummaryResponse>>
{
    public DefaultIdType? CollectionCaseId { get; set; }
    public DefaultIdType? LoanId { get; set; }
    public DefaultIdType? MemberId { get; set; }
    public DefaultIdType? CollectionActionId { get; set; }
    public string? Status { get; set; }
    public DateOnly? PromiseDateFrom { get; set; }
    public DateOnly? PromiseDateTo { get; set; }
    public DateOnly? PromisedPaymentDateFrom { get; set; }
    public DateOnly? PromisedPaymentDateTo { get; set; }
    public decimal? MinPromisedAmount { get; set; }
    public decimal? MaxPromisedAmount { get; set; }
}

public sealed record PromiseToPaySummaryResponse(
    DefaultIdType Id,
    DefaultIdType CollectionCaseId,
    DefaultIdType LoanId,
    DefaultIdType MemberId,
    DefaultIdType? CollectionActionId,
    DateOnly PromiseDate,
    DateOnly PromisedPaymentDate,
    decimal PromisedAmount,
    decimal ActualAmountPaid,
    DateOnly? ActualPaymentDate,
    string Status,
    string? PaymentMethod,
    string? BreachReason,
    int RescheduleCount,
    DefaultIdType RecordedById
);
