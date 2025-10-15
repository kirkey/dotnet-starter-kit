namespace Accounting.Domain.Events.Check;

/// <summary>
/// Event raised when a check is marked as stale (outstanding for too long).
/// </summary>
public record CheckMarkedAsStale(
    DefaultIdType CheckId,
    string CheckNumber,
    DateTime MarkedDate) : DomainEvent;

