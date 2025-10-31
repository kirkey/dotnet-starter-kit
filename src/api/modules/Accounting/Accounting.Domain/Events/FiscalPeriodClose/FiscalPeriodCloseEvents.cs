namespace Accounting.Domain.Events.FiscalPeriodClose;

public record FiscalPeriodCloseInitiated(DefaultIdType Id, string CloseNumber, DefaultIdType PeriodId, string CloseType, string InitiatedBy, DateTime InitiatedDate) : DomainEvent;

public record FiscalPeriodCloseTaskCompleted(DefaultIdType Id, string CloseNumber, string TaskName) : DomainEvent;

public record FiscalPeriodCloseValidationIssueFound(DefaultIdType Id, string CloseNumber, string IssueDescription, string Severity) : DomainEvent;

public record FiscalPeriodCloseValidationIssueResolved(DefaultIdType Id, string CloseNumber, string IssueDescription, string Resolution) : DomainEvent;

public record FiscalPeriodCloseCompleted(DefaultIdType Id, string CloseNumber, DefaultIdType PeriodId, string CloseType, string CompletedBy, DateTime CompletedDate, decimal? FinalNetIncome) : DomainEvent;

public record FiscalPeriodCloseReopened(DefaultIdType Id, string CloseNumber, DefaultIdType PeriodId, string ReopenedBy, string Reason) : DomainEvent;

public record FiscalPeriodCloseDeleted(DefaultIdType Id) : DomainEvent;

