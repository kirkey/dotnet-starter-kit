namespace Accounting.Application.RegulatoryReports.Update.v1;

public class UpdateRegulatoryReportRequest : BaseRequest, IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public string ReportName { get; set; } = null!;
    public string ReportType { get; set; } = null!;
    public string ReportingPeriod { get; set; } = null!;
    public DateTime PeriodStartDate { get; set; }
    public DateTime PeriodEndDate { get; set; }
    public DateTime DueDate { get; set; }
    public string? RegulatoryBody { get; set; }
    public bool RequiresAudit { get; set; }
    public decimal? TotalAssets { get; set; }
    public decimal? TotalLiabilities { get; set; }
    public decimal? TotalEquity { get; set; }
    public decimal? TotalRevenue { get; set; }
    public decimal? TotalExpenses { get; set; }
    public decimal? NetIncome { get; set; }
    public decimal? RateBase { get; set; }
    public decimal? AllowedReturn { get; set; }
}
