namespace Accounting.Domain.Events.Check;

/// <summary>
/// Event raised when a new check is registered in the system.
/// </summary>
public record CheckRegistered(
    DefaultIdType CheckId,
    string CheckNumber,
    string BankAccountCode,
    string Status) : DomainEvent;
