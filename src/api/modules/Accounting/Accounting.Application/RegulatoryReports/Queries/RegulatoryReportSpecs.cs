using Accounting.Domain.Entities;

namespace Accounting.Application.RegulatoryReports.Queries;

public class RegulatoryReportByIdSpec : SingleResultSpecification<RegulatoryReport>
{
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
