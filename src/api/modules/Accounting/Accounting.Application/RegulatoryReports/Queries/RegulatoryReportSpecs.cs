using Accounting.Application.RegulatoryReports.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.RegulatoryReports.Queries;

/// <summary>
/// Specification to retrieve a regulatory report by ID projected to <see cref="RegulatoryReportResponse"/>.
/// Performs database-level projection for optimal performance.
/// </summary>
public class RegulatoryReportByIdSpec : SingleResultSpecification<RegulatoryReport, RegulatoryReportResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegulatoryReportByIdSpec"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the regulatory report to retrieve.</param>
    public RegulatoryReportByIdSpec(DefaultIdType id)
    {
        Query.Where(r => r.Id == id);
    }
}

public class RegulatoryReportByTypeSpec : Specification<RegulatoryReport>
{
    public RegulatoryReportByTypeSpec(string reportType)
    {
        Query.Where(r => r.ReportType == reportType);
    }
}

public class RegulatoryReportByStatusSpec : Specification<RegulatoryReport>
{
    public RegulatoryReportByStatusSpec(string status)
    {
        Query.Where(r => r.Status == status);
    }
}

public class RegulatoryReportByPeriodSpec : Specification<RegulatoryReport>
{
    public RegulatoryReportByPeriodSpec(DateTime startDate, DateTime endDate)
    {
        Query.Where(r => r.PeriodStartDate >= startDate && r.PeriodEndDate <= endDate);
    }
}
