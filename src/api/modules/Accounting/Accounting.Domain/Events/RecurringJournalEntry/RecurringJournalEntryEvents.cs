namespace Accounting.Domain.Events.RecurringJournalEntry;

public sealed record RecurringJournalEntryCreated(
    DefaultIdType Id,
    string TemplateCode,
    string Description,
    RecurrenceFrequency Frequency,
    decimal Amount,
    DateTime StartDate) : DomainEvent;

public sealed record RecurringJournalEntryUpdated(
    DefaultIdType Id,
    string Description,
    decimal Amount) : DomainEvent;

public sealed record RecurringJournalEntryApproved(
    DefaultIdType Id,
    string ApprovedBy,
    DateTime ApprovedDate) : DomainEvent;

public sealed record RecurringJournalEntrySuspended(
    DefaultIdType Id,
    string? Reason) : DomainEvent;

public sealed record RecurringJournalEntryReactivated(DefaultIdType Id) : DomainEvent;

public sealed record RecurringJournalEntryGenerated(
    DefaultIdType Id,
    DefaultIdType JournalEntryId,
    DateTime GeneratedDate,
    DateTime NextRunDate) : DomainEvent;

public sealed record RecurringJournalEntryExpired(DefaultIdType Id) : DomainEvent;

public sealed record RecurringJournalEntryDeleted(DefaultIdType Id) : DomainEvent;
