namespace Accounting.Domain.Events.Check;

/// <summary>
/// Event raised when stop payment is requested on a check.
/// </summary>
public record CheckStopPaymentRequested(
    DefaultIdType CheckId,
    string CheckNumber,
    DateTime StopPaymentDate,
    string StopPaymentReason) : DomainEvent;

