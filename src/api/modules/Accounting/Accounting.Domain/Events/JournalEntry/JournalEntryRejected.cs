namespace Accounting.Domain.Events.JournalEntry;

public record JournalEntryRejected(DefaultIdType JournalEntryId, string RejectedBy, DateTime RejectedDate) : DomainEvent;
