namespace Accounting.Application.RegulatoryReports.Create.v1;

public class RegulatoryReportCreateRequest : BaseRequest, IRequest<DefaultIdType>
{
    public string ReportName { get; set; } = null!;
    public string ReportType { get; set; } = null!; // "FERC Form 1", "FERC Form 2", "FERC Form 6", "EIA Form 861", "State Commission"
    public string ReportingPeriod { get; set; } = null!; // "Annual", "Monthly", "Quarterly"
    public DateTime PeriodStartDate { get; set; }
    public DateTime PeriodEndDate { get; set; }
    public DateTime DueDate { get; set; }
    public string? RegulatoryBody { get; set; } // "FERC", "EIA", "State Commission"
    public bool RequiresAudit { get; set; }
}
