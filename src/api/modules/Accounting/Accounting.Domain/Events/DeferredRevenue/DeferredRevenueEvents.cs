namespace Accounting.Domain.Events.DeferredRevenue;

/// <summary>
/// Event raised when deferred revenue is created.
/// </summary>
public record DeferredRevenueCreated(
    DefaultIdType Id,
    string DeferredRevenueNumber,
    DateTime RecognitionDate,
    decimal Amount,
    string? Description) : DomainEvent;

/// <summary>
/// Event raised when deferred revenue is updated.
/// </summary>
public record DeferredRevenueUpdated(Accounting.Domain.DeferredRevenue DeferredRevenue) : DomainEvent;

/// <summary>
/// Event raised when deferred revenue is deleted.
/// </summary>
public record DeferredRevenueDeleted(DefaultIdType Id, string DeferredRevenueNumber) : DomainEvent;

/// <summary>
/// Event raised when deferred revenue is fully recognized.
/// </summary>
public record DeferredRevenueRecognized(
    DefaultIdType Id,
    string DeferredRevenueNumber,
    decimal Amount,
    DateTime RecognizedDate) : DomainEvent;

/// <summary>
/// Event raised when deferred revenue is partially recognized.
/// </summary>
public record DeferredRevenuePartiallyRecognized(
    DefaultIdType Id,
    string DeferredRevenueNumber,
    decimal RecognizedAmount,
    decimal RemainingAmount,
    DateTime RecognizedDate) : DomainEvent;

/// <summary>
/// Event raised when deferred revenue amount is adjusted.
/// </summary>
public record DeferredRevenueAdjusted(
    DefaultIdType Id,
    string DeferredRevenueNumber,
    decimal OldAmount,
    decimal NewAmount,
    string? Reason) : DomainEvent;
