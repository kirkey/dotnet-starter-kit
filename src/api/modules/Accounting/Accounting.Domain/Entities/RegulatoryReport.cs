using Accounting.Domain.Events.RegulatoryReport;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a regulatory report filing with compliance tracking, financial data aggregation, and submission workflow management.
/// </summary>
/// <remarks>
/// Use cases:
/// - Generate and submit regulatory reports to FERC, EIA, state public utility commissions.
/// - Track compliance deadlines and filing status for audit and regulatory oversight.
/// - Aggregate financial data from general ledger for standardized regulatory reporting formats.
/// - Support multi-period reporting (annual, quarterly, monthly) with proper data validation.
/// - Enable regulatory report review and approval workflows before submission.
/// - Maintain historical regulatory filings for compliance audits and trend analysis.
/// - Support automated data extraction from accounting systems for report generation.
/// - Track regulatory correspondence and amendment requirements.
/// 
/// Default values:
/// - ReportName: required display name (example: "FERC Form 1 - 2025 Annual Report")
/// - ReportType: required regulatory classification (example: "FERC Form 1", "EIA Form 861")
/// - ReportingPeriod: required frequency (example: "Annual", "Quarterly", "Monthly")
/// - PeriodStart: required start date of reporting period (example: 2025-01-01)
/// - PeriodEnd: required end date of reporting period (example: 2025-12-31)
/// - DueDate: required regulatory filing deadline (example: 2026-04-18 for annual reports)
/// - Status: "Draft" (new reports start in draft status)
/// - SubmissionDate: null (set when report is submitted)
/// - TotalRevenue: null (calculated from financial data)
/// - TotalExpenses: null (calculated from financial data)
/// - NetIncome: null (calculated as revenue minus expenses)
/// 
/// Business rules:
/// - PeriodEnd must be after PeriodStart
/// - DueDate should be after PeriodEnd for filing deadline validation
/// - Cannot submit reports with "Draft" status without approval
/// - Financial amounts must reconcile with general ledger data
/// - Report amendments require proper documentation and approval
/// - Submission creates immutable filing record for compliance
/// - Late filings require explanation and penalty tracking
/// - Report data must comply with regulatory formatting standards
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.RegulatoryReport.RegulatoryReportCreated"/>
/// <seealso cref="Accounting.Domain.Events.RegulatoryReport.RegulatoryReportSubmitted"/>
/// <seealso cref="Accounting.Domain.Events.RegulatoryReport.RegulatoryReportApproved"/>
/// <seealso cref="Accounting.Domain.Events.RegulatoryReport.RegulatoryReportRejected"/>
/// <seealso cref="Accounting.Domain.Events.RegulatoryReport.RegulatoryReportAmended"/>
public class RegulatoryReport : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Report display name.
    /// </summary>
    public string ReportName { get; private set; }

    /// <summary>
    /// Report type/series (e.g., "FERC Form 1").
    /// </summary>
    public string ReportType { get; private set; } // "FERC Form 1", "FERC Form 2", "FERC Form 6", "EIA Form 861", "State Commission"

    /// <summary>
    /// Frequency label for reporting (Annual, Monthly, Quarterly).
    /// </summary>
    public string ReportingPeriod { get; private set; } // "Annual", "Monthly", "Quarterly"

    /// <summary>
    /// Start date of the reporting period.
    /// </summary>
    public DateTime PeriodStartDate { get; private set; }

    /// <summary>
    /// End date of the reporting period.
    /// </summary>
    public DateTime PeriodEndDate { get; private set; }

    /// <summary>
    /// Filing due date.
    /// </summary>
    public DateTime DueDate { get; private set; }

    /// <summary>
    /// Date the report was submitted, if applicable.
    /// </summary>
    public DateTime? SubmissionDate { get; private set; }

    /// <summary>
    /// Workflow status: Draft, In Review, Submitted, Approved, Rejected.
    /// </summary>
    public string Status { get; private set; } // "Draft", "In Review", "Submitted", "Approved", "Rejected"

    /// <summary>
    /// Regulatory body (e.g., FERC, EIA, State Commission).
    /// </summary>
    public string? RegulatoryBody { get; private set; } // "FERC", "EIA", "State Commission"

    /// <summary>
    /// Filing reference number if assigned.
    /// </summary>
    public string? FilingNumber { get; private set; }

    /// <summary>
    /// Optional total assets figure.
    /// </summary>
    public decimal? TotalAssets { get; private set; }

    /// <summary>
    /// Optional total liabilities figure.
    /// </summary>
    public decimal? TotalLiabilities { get; private set; }

    /// <summary>
    /// Optional total equity figure.
    /// </summary>
    public decimal? TotalEquity { get; private set; }

    /// <summary>
    /// Optional total revenue figure.
    /// </summary>
    public decimal? TotalRevenue { get; private set; }

    /// <summary>
    /// Optional total expenses figure.
    /// </summary>
    public decimal? TotalExpenses { get; private set; }

    /// <summary>
    /// Optional net income figure.
    /// </summary>
    public decimal? NetIncome { get; private set; }

    /// <summary>
    /// Optional rate base.
    /// </summary>
    public decimal? RateBase { get; private set; }

    /// <summary>
    /// Optional allowed return on rate base.
    /// </summary>
    public decimal? AllowedReturn { get; private set; }

    /// <summary>
    /// Local path/URI to the generated report file (if any).
    /// </summary>
    public string? FilePath { get; private set; } // Path to the report file

    /// <summary>
    /// Name/identifier of the preparer.
    /// </summary>
    public string? PreparedBy { get; private set; }

    /// <summary>
    /// Name/identifier of the reviewer.
    /// </summary>
    public string? ReviewedBy { get; private set; }

    /// <summary>
    /// Name/identifier of the approver.
    /// </summary>
    public string? ApprovedBy { get; private set; }

    /// <summary>
    /// Whether this report requires an independent audit.
    /// </summary>
    public bool RequiresAudit { get; private set; }

    /// <summary>
    /// Audit firm name, when applicable.
    /// </summary>
    public string? AuditFirm { get; private set; }

    /// <summary>
    /// Audit completion date, when applicable.
    /// </summary>
    public DateTime? AuditDate { get; private set; }
    
    private RegulatoryReport()
    {
        ReportName = string.Empty;
        ReportType = string.Empty;
        ReportingPeriod = string.Empty;
        Status = "Draft";
    }

    private RegulatoryReport(string reportName, string reportType, string reportingPeriod,
        DateTime periodStartDate, DateTime periodEndDate, DateTime dueDate,
        string? regulatoryBody = null, bool requiresAudit = false,
        string? description = null, string? notes = null)
    {
        ReportName = reportName.Trim();
        ReportType = reportType.Trim();
        ReportingPeriod = reportingPeriod.Trim();
        PeriodStartDate = periodStartDate;
        PeriodEndDate = periodEndDate;
        DueDate = dueDate;
        Status = "Draft";
        RegulatoryBody = regulatoryBody?.Trim();
        RequiresAudit = requiresAudit;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new RegulatoryReportCreated(Id, ReportName, ReportType, PeriodStartDate, PeriodEndDate, Description, Notes));
    }

    /// <summary>
    /// Create a regulatory report and enforce period/due date validation and required fields.
    /// </summary>
    public static RegulatoryReport Create(string reportName, string reportType, string reportingPeriod,
        DateTime periodStartDate, DateTime periodEndDate, DateTime dueDate,
        string? regulatoryBody = null, bool requiresAudit = false,
        string? description = null, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(reportName))
            throw new RegulatoryReportInvalidException("Report name cannot be empty");
        
        if (string.IsNullOrWhiteSpace(reportType))
            throw new RegulatoryReportInvalidException("Report type cannot be empty");
        
        if (periodStartDate >= periodEndDate)
            throw new RegulatoryReportInvalidException("Period start date must be before end date");
        
        if (dueDate < periodEndDate)
            throw new RegulatoryReportInvalidException("Due date cannot be before period end date");

        return new RegulatoryReport(reportName, reportType, reportingPeriod, periodStartDate, periodEndDate, dueDate, regulatoryBody, requiresAudit, description, notes);
    }

    /// <summary>
    /// Set top-level financial aggregates for the report.
    /// </summary>
    public void UpdateFinancialData(decimal? totalAssets = null, decimal? totalLiabilities = null,
        decimal? totalEquity = null, decimal? totalRevenue = null, decimal? totalExpenses = null,
        decimal? netIncome = null, decimal? rateBase = null, decimal? allowedReturn = null)
    {
        TotalAssets = totalAssets;
        TotalLiabilities = totalLiabilities;
        TotalEquity = totalEquity;
        TotalRevenue = totalRevenue;
        TotalExpenses = totalExpenses;
        NetIncome = netIncome;
        RateBase = rateBase;
        AllowedReturn = allowedReturn;

        QueueDomainEvent(new RegulatoryReportFinancialDataUpdated(Id, TotalAssets, TotalRevenue, NetIncome));
    }

    /// <summary>
    /// Move from In Review to Submitted and set submission metadata.
    /// </summary>
    public void Submit(string submittedBy, string? filingNumber = null)
    {
        if (Status != "In Review")
            throw new RegulatoryReportInvalidException("Report must be in review status before submission");

        Status = "Submitted";
        SubmissionDate = DateTime.UtcNow;
        FilingNumber = filingNumber?.Trim();

        QueueDomainEvent(new RegulatoryReportSubmitted(Id, ReportName, SubmissionDate.Value, submittedBy));
    }

    /// <summary>
    /// Move from Draft to In Review and set reviewer metadata.
    /// </summary>
    public void MarkForReview(string reviewedBy)
    {
        if (Status != "Draft")
            throw new RegulatoryReportInvalidException("Only draft reports can be marked for review");

        Status = "In Review";
        ReviewedBy = reviewedBy.Trim();

        QueueDomainEvent(new RegulatoryReportMarkedForReview(Id, ReportName, reviewedBy));
    }

    /// <summary>
    /// Approve a submitted report.
    /// </summary>
    public void Approve(string approvedBy)
    {
        if (Status != "Submitted")
            throw new RegulatoryReportInvalidException("Only submitted reports can be approved");

        Status = "Approved";
        ApprovedBy = approvedBy.Trim();

        QueueDomainEvent(new RegulatoryReportApproved(Id, ReportName, approvedBy));
    }

    /// <summary>
    /// Reject a submitted report and append reason to notes.
    /// </summary>
    public void Reject(string reason)
    {
        if (Status != "Submitted")
            throw new RegulatoryReportInvalidException("Only submitted reports can be rejected");

        Status = "Rejected";
        Notes = $"{Notes}\nRejection Reason: {reason}";

        QueueDomainEvent(new RegulatoryReportRejected(Id, ReportName, reason));
    }
}
