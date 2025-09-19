namespace Accounting.Application.RegulatoryReports.Responses;

/// <summary>
/// Response model representing a regulatory report entity.
/// Contains comprehensive regulatory reporting information including financial data and submission details.
/// </summary>
public class RegulatoryReportResponse
{
    /// <summary>
    /// Unique identifier for the regulatory report.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// Name of the regulatory report.
    /// </summary>
    public string ReportName { get; set; } = null!;
    
    /// <summary>
    /// Type or category of regulatory report.
    /// </summary>
    public string ReportType { get; set; } = null!;
    
    /// <summary>
    /// Reporting period description (e.g., "Q1 2024", "Annual 2023").
    /// </summary>
    public string ReportingPeriod { get; set; } = null!;
    
    /// <summary>
    /// Start date of the reporting period.
    /// </summary>
    public DateTime PeriodStartDate { get; set; }
    
    /// <summary>
    /// End date of the reporting period.
    /// </summary>
    public DateTime PeriodEndDate { get; set; }
    
    /// <summary>
    /// Due date for submission of this regulatory report.
    /// </summary>
    public DateTime DueDate { get; set; }
    
    /// <summary>
    /// Date when the report was actually submitted (if applicable).
    /// </summary>
    public DateTime? SubmissionDate { get; set; }
    
    /// <summary>
    /// Current status of the report (e.g., "Draft", "Submitted", "Approved").
    /// </summary>
    public string Status { get; set; } = null!;
    
    /// <summary>
    /// Name of the regulatory body that requires this report.
    /// </summary>
    public string? RegulatoryBody { get; set; }
    
    /// <summary>
    /// Filing number assigned by the regulatory body.
    /// </summary>
    public string? FilingNumber { get; set; }
    
    /// <summary>
    /// Total assets as reported in the regulatory filing.
    /// </summary>
    public decimal? TotalAssets { get; set; }
    
    /// <summary>
    /// Total liabilities as reported in the regulatory filing.
    /// </summary>
    public decimal? TotalLiabilities { get; set; }
    
    /// <summary>
    /// Total equity as reported in the regulatory filing.
    /// </summary>
    public decimal? TotalEquity { get; set; }
    
    /// <summary>
    /// Total revenue for the reporting period.
    /// </summary>
    public decimal? TotalRevenue { get; set; }
    
    /// <summary>
    /// Total expenses for the reporting period.
    /// </summary>
    public decimal? TotalExpenses { get; set; }
    
    /// <summary>
    /// Net income for the reporting period.
    /// </summary>
    public decimal? NetIncome { get; set; }
    
    /// <summary>
    /// Rate base used for utility rate calculations.
    /// </summary>
    public decimal? RateBase { get; set; }
    
    /// <summary>
    /// Allowed return on investment as approved by regulators.
    /// </summary>
    public decimal? AllowedReturn { get; set; }
    
    /// <summary>
    /// Name of the person who prepared the report.
    /// </summary>
    public string? PreparedBy { get; set; }
    
    /// <summary>
    /// Name of the person who reviewed the report.
    /// </summary>
    public string? ReviewedBy { get; set; }
    
    /// <summary>
    /// Name of the person who approved the report.
    /// </summary>
    public string? ApprovedBy { get; set; }
    
    /// <summary>
    /// Indicates whether this report requires an external audit.
    /// </summary>
    public bool RequiresAudit { get; set; }
    
    /// <summary>
    /// Name of the audit firm conducting the audit (if applicable).
    /// </summary>
    public string? AuditFirm { get; set; }
    
    /// <summary>
    /// Date when the audit was completed (if applicable).
    /// </summary>
    public DateTime? AuditDate { get; set; }
    
    /// <summary>
    /// Description or additional details about the regulatory report.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Additional notes or comments about the regulatory report.
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// Date when the report record was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }
    
    /// <summary>
    /// Date when the report record was last modified.
    /// </summary>
    public DateTime? LastModifiedOn { get; set; }
}
