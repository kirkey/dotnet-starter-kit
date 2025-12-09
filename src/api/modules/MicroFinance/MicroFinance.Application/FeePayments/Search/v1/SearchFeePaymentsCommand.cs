using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Search.v1;

public class SearchFeePaymentsCommand : PaginationFilter, IRequest<PagedList<FeePaymentSummaryResponse>>
{
    public DefaultIdType? FeeChargeId { get; set; }
    public string? Reference { get; set; }
    public string? PaymentMethod { get; set; }
    public string? PaymentSource { get; set; }
    public string? Status { get; set; }
    public DateOnly? PaymentDateFrom { get; set; }
    public DateOnly? PaymentDateTo { get; set; }
}

public sealed record FeePaymentSummaryResponse(
    DefaultIdType Id,
    DefaultIdType FeeChargeId,
    string Reference,
    DateOnly PaymentDate,
    decimal Amount,
    string PaymentMethod,
    string PaymentSource,
    string Status);
