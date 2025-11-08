namespace Accounting.Domain.Events.GeneralLedger;

/// <summary>
/// Event raised when a general ledger entry is created.
/// </summary>
public sealed record GeneralLedgerEntryCreated(
    DefaultIdType Id,
    DefaultIdType EntryId,
    DefaultIdType AccountId,
    decimal Debit,
    decimal Credit,
    string UsoaClass,
    DateTime TransactionDate) : DomainEvent;

/// <summary>
/// Event raised when a general ledger entry is posted to the ledger.
/// </summary>
public sealed record GeneralLedgerPosted(
    DefaultIdType Id,
    string AccountCode,
    DateTime TransactionDate,
    decimal Debit,
    decimal Credit) : DomainEvent;

/// <summary>
/// Event raised when a general ledger entry is updated.
/// </summary>
public sealed record GeneralLedgerEntryUpdated(
    DefaultIdType Id,
    DefaultIdType EntryId,
    DefaultIdType AccountId,
    decimal Debit,
    decimal Credit,
    string UsoaClass) : DomainEvent;

/// <summary>
/// Event raised when a general ledger entry is reversed.
/// </summary>
public sealed record GeneralLedgerReversed(
    DefaultIdType Id,
    string AccountCode,
    DateTime TransactionDate,
    string Reason) : DomainEvent;

/// <summary>
/// Event raised when a general ledger entry is adjusted.
/// </summary>
public sealed record GeneralLedgerAdjusted(
    DefaultIdType Id,
    string AccountCode,
    DateTime TransactionDate,
    decimal OldDebit,
    decimal OldCredit,
    decimal NewDebit,
    decimal NewCredit,
    string Reason) : DomainEvent;

