namespace Accounting.Domain.Events.Check;

/// <summary>
/// Event raised when check details are updated.
/// </summary>
public record CheckUpdated(
    DefaultIdType CheckId,
    string CheckNumber,
    string BankAccountCode) : DomainEvent;
