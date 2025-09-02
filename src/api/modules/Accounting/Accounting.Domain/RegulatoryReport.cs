using Accounting.Domain.Events.RegulatoryReport;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;

public class RegulatoryReport : AuditableEntity, IAggregateRoot
{
    public string ReportName { get; private set; }
    public string ReportType { get; private set; } // "FERC Form 1", "FERC Form 2", "FERC Form 6", "EIA Form 861", "State Commission"
    public string ReportingPeriod { get; private set; } // "Annual", "Monthly", "Quarterly"
    public DateTime PeriodStartDate { get; private set; }
    public DateTime PeriodEndDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? SubmissionDate { get; private set; }
    public string Status { get; private set; } // "Draft", "In Review", "Submitted", "Approved", "Rejected"
    public string? RegulatoryBody { get; private set; } // "FERC", "EIA", "State Commission"
    public string? FilingNumber { get; private set; }
    public decimal? TotalAssets { get; private set; }
    public decimal? TotalLiabilities { get; private set; }
    public decimal? TotalEquity { get; private set; }
    public decimal? TotalRevenue { get; private set; }
    public decimal? TotalExpenses { get; private set; }
    public decimal? NetIncome { get; private set; }
    public decimal? RateBase { get; private set; }
    public decimal? AllowedReturn { get; private set; }
    public string? FilePath { get; private set; } // Path to the report file
    public string? PreparedBy { get; private set; }
    public string? ReviewedBy { get; private set; }
    public string? ApprovedBy { get; private set; }
    public bool RequiresAudit { get; private set; }
    public string? AuditFirm { get; private set; }
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

    public void Submit(string submittedBy, string? filingNumber = null)
    {
        if (Status != "In Review")
            throw new RegulatoryReportInvalidException("Report must be in review status before submission");

        Status = "Submitted";
        SubmissionDate = DateTime.UtcNow;
        FilingNumber = filingNumber?.Trim();

        QueueDomainEvent(new RegulatoryReportSubmitted(Id, ReportName, SubmissionDate.Value, submittedBy));
    }

    public void MarkForReview(string reviewedBy)
    {
        if (Status != "Draft")
            throw new RegulatoryReportInvalidException("Only draft reports can be marked for review");

        Status = "In Review";
        ReviewedBy = reviewedBy.Trim();

        QueueDomainEvent(new RegulatoryReportMarkedForReview(Id, ReportName, reviewedBy));
    }

    public void Approve(string approvedBy)
    {
        if (Status != "Submitted")
            throw new RegulatoryReportInvalidException("Only submitted reports can be approved");

        Status = "Approved";
        ApprovedBy = approvedBy.Trim();

        QueueDomainEvent(new RegulatoryReportApproved(Id, ReportName, approvedBy));
    }

    public void Reject(string reason)
    {
        if (Status != "Submitted")
            throw new RegulatoryReportInvalidException("Only submitted reports can be rejected");

        Status = "Rejected";
        Notes = $"{Notes}\nRejection Reason: {reason}";

        QueueDomainEvent(new RegulatoryReportRejected(Id, ReportName, reason));
    }
}
