namespace Accounting.Domain.Events.Check;

/// <summary>
/// Event raised when a check is issued for payment.
/// </summary>
public record CheckIssued(
    DefaultIdType CheckId,
    string CheckNumber,
    decimal Amount,
    string PayeeName,
    DateTime IssuedDate,
    DefaultIdType? VendorId,
    DefaultIdType? PayeeId) : DomainEvent;

