namespace Accounting.Domain.Events.BankReconciliation;

public sealed record BankReconciliationCreated(
    DefaultIdType Id,
    DefaultIdType BankAccountId,
    DateTime ReconciliationDate,
    decimal StatementBalance,
    decimal BookBalance) : DomainEvent;

public sealed record BankReconciliationUpdated(
    DefaultIdType Id,
    decimal OutstandingChecksTotal,
    decimal DepositsInTransitTotal,
    decimal BankErrors,
    decimal BookErrors,
    decimal AdjustedBalance) : DomainEvent;

public sealed record BankReconciliationStarted(DefaultIdType Id) : DomainEvent;

public sealed record BankReconciliationCompleted(
    DefaultIdType Id,
    string ReconciledBy,
    DateTime ReconciledDate) : DomainEvent;

public sealed record BankReconciliationApproved(
    DefaultIdType Id,
    string ApprovedBy,
    DateTime ApprovedDate) : DomainEvent;

public sealed record BankReconciliationRejected(
    DefaultIdType Id,
    string RejectedBy,
    string? Reason) : DomainEvent;

public sealed record BankReconciliationDeleted(DefaultIdType Id) : DomainEvent;
