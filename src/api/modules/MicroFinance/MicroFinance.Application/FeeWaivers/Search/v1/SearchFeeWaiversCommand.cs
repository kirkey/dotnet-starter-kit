using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Search.v1;

public class SearchFeeWaiversCommand : PaginationFilter, IRequest<PagedList<FeeWaiverSummaryResponse>>
{
    public DefaultIdType? FeeChargeId { get; set; }
    public string? Reference { get; set; }
    public string? WaiverType { get; set; }
    public string? Status { get; set; }
    public DateOnly? RequestDateFrom { get; set; }
    public DateOnly? RequestDateTo { get; set; }
}

public sealed record FeeWaiverSummaryResponse(
    DefaultIdType Id,
    DefaultIdType FeeChargeId,
    string Reference,
    string WaiverType,
    DateOnly RequestDate,
    decimal OriginalAmount,
    decimal WaivedAmount,
    string WaiverReason,
    string Status,
    string? ApprovedBy,
    DateOnly? ApprovalDate);
