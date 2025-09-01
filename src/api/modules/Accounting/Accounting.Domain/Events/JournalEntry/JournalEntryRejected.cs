using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.JournalEntry;

using System;

public record JournalEntryRejected(DefaultIdType JournalEntryId, string RejectedBy, DateTime RejectedDate) : DomainEvent;
