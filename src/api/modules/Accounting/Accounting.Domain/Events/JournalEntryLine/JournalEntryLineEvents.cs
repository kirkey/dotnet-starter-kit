namespace Accounting.Domain.Events.JournalEntryLine;

/// <summary>
/// Domain event raised when a journal entry line is created.
/// </summary>
public record JournalEntryLineCreated(
    DefaultIdType Id, 
    DefaultIdType JournalEntryId, 
    DefaultIdType AccountId, 
    decimal DebitAmount, 
    decimal CreditAmount) : DomainEvent;

/// <summary>
/// Domain event raised when a journal entry line is updated.
/// </summary>
public record JournalEntryLineUpdated(Entities.JournalEntryLine JournalEntryLine) : DomainEvent;

/// <summary>
/// Domain event raised when a journal entry line is deleted.
/// </summary>
public record JournalEntryLineDeleted(DefaultIdType Id, DefaultIdType JournalEntryId) : DomainEvent;

