namespace Accounting.Domain.Events.JournalEntry;

public record JournalEntryCreated(DefaultIdType Id, DateTime Date, string ReferenceNumber, string Description, string Source) : DomainEvent;

public record JournalEntryUpdated(Accounting.Domain.JournalEntry JournalEntry) : DomainEvent;

public record JournalEntryDeleted(DefaultIdType Id) : DomainEvent;

public record JournalEntryPosted(DefaultIdType Id, DateTime PostedDate) : DomainEvent;

public record JournalEntryReversed(DefaultIdType Id, DateTime ReversalDate, string Reason) : DomainEvent;

public record JournalEntryLineAdded(DefaultIdType JournalEntryId, DefaultIdType LineId, DefaultIdType AccountId, decimal DebitAmount, decimal CreditAmount) : DomainEvent;
