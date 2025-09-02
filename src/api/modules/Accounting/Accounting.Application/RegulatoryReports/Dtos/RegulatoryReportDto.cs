namespace Accounting.Application.RegulatoryReports.Dtos;

public class RegulatoryReportDto
{
    public DefaultIdType Id { get; set; }
    public string ReportName { get; set; } = null!;
    public string ReportType { get; set; } = null!;
    public string ReportingPeriod { get; set; } = null!;
    public DateTime PeriodStartDate { get; set; }
    public DateTime PeriodEndDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? SubmissionDate { get; set; }
    public string Status { get; set; } = null!;
    public string? RegulatoryBody { get; set; }
    public string? FilingNumber { get; set; }
    public decimal? TotalAssets { get; set; }
    public decimal? TotalLiabilities { get; set; }
    public decimal? TotalEquity { get; set; }
    public decimal? TotalRevenue { get; set; }
    public decimal? TotalExpenses { get; set; }
    public decimal? NetIncome { get; set; }
    public decimal? RateBase { get; set; }
    public decimal? AllowedReturn { get; set; }
    public string? PreparedBy { get; set; }
    public string? ReviewedBy { get; set; }
    public string? ApprovedBy { get; set; }
    public bool RequiresAudit { get; set; }
    public string? AuditFirm { get; set; }
    public DateTime? AuditDate { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}
