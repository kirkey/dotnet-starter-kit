using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.RegulatoryReport;

public record RegulatoryReportCreated(
    DefaultIdType Id,
    string ReportName,
    string ReportType,
    DateTime PeriodStartDate,
    DateTime PeriodEndDate,
    string? Description,
    string? Notes) : DomainEvent;
