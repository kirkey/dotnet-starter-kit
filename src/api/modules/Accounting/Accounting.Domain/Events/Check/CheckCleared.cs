namespace Accounting.Domain.Events.Check;

/// <summary>
/// Event raised when a check clears the bank.
/// </summary>
public record CheckCleared(
    DefaultIdType CheckId,
    string CheckNumber,
    decimal Amount,
    DateTime ClearedDate) : DomainEvent;

