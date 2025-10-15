namespace Accounting.Domain.Events.Check;

/// <summary>
/// Event raised when a check is printed.
/// </summary>
public record CheckPrinted(
    DefaultIdType CheckId,
    string CheckNumber,
    DateTime PrintedDate,
    string PrintedBy) : DomainEvent;

