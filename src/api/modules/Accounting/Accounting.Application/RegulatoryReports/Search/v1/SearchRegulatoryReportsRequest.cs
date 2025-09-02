using Accounting.Application.RegulatoryReports.Dtos;
using FSH.Framework.Core.Paging;
using MediatR;

namespace Accounting.Application.RegulatoryReports.Search.v1;

public class SearchRegulatoryReportsRequest : PaginationFilter, IRequest<PagedList<RegulatoryReportDto>>
{
    public string? ReportType { get; set; }
    public string? Status { get; set; }
    public string? RegulatoryBody { get; set; }
    public DateTime? PeriodStartDate { get; set; }
    public DateTime? PeriodEndDate { get; set; }
}
