namespace Accounting.Domain.Events.WriteOff;

public sealed record WriteOffCreated(
    DefaultIdType Id,
    string ReferenceNumber,
    DateTime WriteOffDate,
    WriteOffType WriteOffType,
    decimal Amount) : DomainEvent;

public sealed record WriteOffUpdated(DefaultIdType Id) : DomainEvent;

public sealed record WriteOffApproved(
    DefaultIdType Id,
    string ApprovedBy,
    DateTime ApprovedDate) : DomainEvent;

public sealed record WriteOffRejected(
    DefaultIdType Id,
    string RejectedBy,
    string? Reason) : DomainEvent;

public sealed record WriteOffPosted(
    DefaultIdType Id,
    DefaultIdType JournalEntryId) : DomainEvent;

public sealed record WriteOffRecovered(
    DefaultIdType Id,
    decimal RecoveryAmount,
    decimal TotalRecovered,
    bool IsFullyRecovered,
    DefaultIdType? RecoveryJournalEntryId) : DomainEvent;

public sealed record WriteOffReversed(
    DefaultIdType Id,
    string? Reason) : DomainEvent;

public sealed record WriteOffDeleted(DefaultIdType Id) : DomainEvent;
