using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.JournalEntry;

public record JournalEntryApproved(DefaultIdType JournalEntryId, string ApprovedBy, DateTime ApprovedDate) : DomainEvent;
