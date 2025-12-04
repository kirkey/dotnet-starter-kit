using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>
/// Domain events for Accounting Integration entities.
/// </summary>
/// 
// ChartOfAccount Events
public sealed record ChartOfAccountCreated(ChartOfAccount Account) : DomainEvent;
public sealed record ChartOfAccountUpdated(ChartOfAccount Account) : DomainEvent;

// JournalEntry Events
public sealed record JournalEntryCreated(JournalEntry Entry) : DomainEvent;
public sealed record JournalEntryUpdated(JournalEntry Entry) : DomainEvent;
public sealed record JournalEntrySubmitted(Guid EntryId, string EntryNumber) : DomainEvent;
public sealed record JournalEntryApproved(Guid EntryId, string EntryNumber) : DomainEvent;
public sealed record JournalEntryPosted(Guid EntryId, string EntryNumber, decimal TotalAmount) : DomainEvent;
public sealed record JournalEntryReversed(Guid EntryId, Guid ReversalEntryId) : DomainEvent;

// AccountingPeriod Events
public sealed record AccountingPeriodCreated(AccountingPeriod Period) : DomainEvent;
public sealed record AccountingPeriodUpdated(AccountingPeriod Period) : DomainEvent;
public sealed record AccountingPeriodClosed(Guid PeriodId, string Name, decimal ClosingBalance) : DomainEvent;
public sealed record AccountingPeriodReopened(Guid PeriodId, string Name) : DomainEvent;
public sealed record AccountingPeriodLocked(Guid PeriodId, string Name) : DomainEvent;
