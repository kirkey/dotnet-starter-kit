using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Search.v1;

public class SearchCreditBureauInquiriesCommand : PaginationFilter, IRequest<PagedList<CreditBureauInquirySummaryResponse>>
{
    public DefaultIdType? MemberId { get; set; }
    public DefaultIdType? LoanId { get; set; }
    public string? InquiryNumber { get; set; }
    public string? BureauName { get; set; }
    public string? Purpose { get; set; }
    public string? Status { get; set; }
    public DateTime? InquiryDateFrom { get; set; }
    public DateTime? InquiryDateTo { get; set; }
    public DefaultIdType? RequestedByUserId { get; set; }
}

public sealed record CreditBureauInquirySummaryResponse(
    DefaultIdType Id,
    DefaultIdType MemberId,
    DefaultIdType? LoanId,
    string InquiryNumber,
    string BureauName,
    string Purpose,
    DateTime InquiryDate,
    string? RequestedBy,
    string? ReferenceNumber,
    string Status,
    DateTime? ResponseReceivedAt
);
