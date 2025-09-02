using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.RegulatoryReport;

public record RegulatoryReportFinancialDataUpdated(
    DefaultIdType Id,
    decimal? TotalAssets,
    decimal? TotalRevenue,
    decimal? NetIncome) : DomainEvent;

public record RegulatoryReportSubmitted(
    DefaultIdType Id,
    string ReportName,
    DateTime SubmissionDate,
    string SubmittedBy) : DomainEvent;

public record RegulatoryReportMarkedForReview(
    DefaultIdType Id,
    string ReportName,
    string ReviewedBy) : DomainEvent;

public record RegulatoryReportApproved(
    DefaultIdType Id,
    string ReportName,
    string ApprovedBy) : DomainEvent;

public record RegulatoryReportRejected(
    DefaultIdType Id,
    string ReportName,
    string Reason) : DomainEvent;
