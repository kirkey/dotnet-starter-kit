namespace Accounting.Domain.Events.AccountReconciliation;

/// <summary>
/// Event raised when an account reconciliation is created.
/// </summary>
/// <param name="Id">Reconciliation ID</param>
/// <param name="GeneralLedgerAccountId">GL account being reconciled</param>
/// <param name="AccountingPeriodId">Accounting period</param>
/// <param name="GlBalance">GL balance</param>
/// <param name="SubsidiaryLedgerBalance">Subsidiary ledger balance</param>
/// <param name="Variance">Calculated variance</param>
public sealed record AccountReconciliationCreated(
    DefaultIdType Id,
    DefaultIdType GeneralLedgerAccountId,
    DefaultIdType AccountingPeriodId,
    decimal GlBalance,
    decimal SubsidiaryLedgerBalance,
    decimal Variance) : DomainEvent;

/// <summary>
/// Event raised when account reconciliation balances are updated.
/// </summary>
public sealed record AccountReconciliationUpdated(
    DefaultIdType Id,
    DefaultIdType GeneralLedgerAccountId,
    decimal GlBalance,
    decimal SubsidiaryLedgerBalance,
    decimal Variance) : DomainEvent;

/// <summary>
/// Event raised when adjusting entries are recorded for reconciliation.
/// </summary>
public sealed record AdjustingEntriesRecorded(
    DefaultIdType Id,
    DefaultIdType GeneralLedgerAccountId) : DomainEvent;

/// <summary>
/// Event raised when account reconciliation is approved.
/// </summary>
public sealed record AccountReconciliationApproved(
    DefaultIdType Id,
    DefaultIdType GeneralLedgerAccountId,
    string ApprovedBy,
    DateTime ApprovedOn) : DomainEvent;

/// <summary>
/// Event raised when account reconciliation is rejected.
/// </summary>
public sealed record AccountReconciliationRejected(
    DefaultIdType Id,
    DefaultIdType GeneralLedgerAccountId,
    string RejectedBy,
    string? Reason) : DomainEvent;

/// <summary>
/// Event raised when account reconciliation is reopened for corrections.
/// </summary>
public sealed record AccountReconciliationReopened(
    DefaultIdType Id,
    DefaultIdType GeneralLedgerAccountId) : DomainEvent;

