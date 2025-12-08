using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauReports.Search.v1;

public class SearchCreditBureauReportsCommand : PaginationFilter, IRequest<PagedList<CreditBureauReportSummaryResponse>>
{
    public DefaultIdType? MemberId { get; set; }
    public DefaultIdType? InquiryId { get; set; }
    public string? ReportNumber { get; set; }
    public string? BureauName { get; set; }
    public string? RiskGrade { get; set; }
    public string? Status { get; set; }
    public DateTime? ReportDateFrom { get; set; }
    public DateTime? ReportDateTo { get; set; }
    public int? CreditScoreMin { get; set; }
    public int? CreditScoreMax { get; set; }
}

public sealed record CreditBureauReportSummaryResponse(
    DefaultIdType Id,
    DefaultIdType MemberId,
    DefaultIdType? InquiryId,
    string ReportNumber,
    string BureauName,
    DateTime ReportDate,
    DateTime? ExpiryDate,
    int? CreditScore,
    string? RiskGrade,
    int? ActiveAccounts);
