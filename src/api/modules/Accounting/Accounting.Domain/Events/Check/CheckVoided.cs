namespace Accounting.Domain.Events.Check;

/// <summary>
/// Event raised when a check is voided.
/// </summary>
public record CheckVoided(
    DefaultIdType CheckId,
    string CheckNumber,
    DateTime VoidedDate,
    string VoidReason) : DomainEvent;

