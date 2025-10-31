using Accounting.Application.RegulatoryReports.Responses;
using Accounting.Application.RegulatoryReports.Search.v1;

namespace Accounting.Application.RegulatoryReports.Specs;

public class SearchRegulatoryReportsSpec : EntitiesByPaginationFilterSpec<RegulatoryReport, RegulatoryReportResponse>
{
    public SearchRegulatoryReportsSpec(SearchRegulatoryReportsRequest request)
        : base(request) =>
        Query
            .OrderBy(r => r.DueDate, !request.HasOrderBy())
            .Where(r => r.ReportType.Contains(request.ReportType!), !string.IsNullOrEmpty(request.ReportType))
            .Where(r => r.Status.Contains(request.Status!), !string.IsNullOrEmpty(request.Status))
            .Where(r => r.RegulatoryBody!.Contains(request.RegulatoryBody!), !string.IsNullOrEmpty(request.RegulatoryBody))
            .Where(r => r.PeriodStartDate >= request.PeriodStartDate, request.PeriodStartDate.HasValue)
            .Where(r => r.PeriodEndDate <= request.PeriodEndDate, request.PeriodEndDate.HasValue);
}
