namespace Accounting.Application.Checks.StopPayment.v1;

/// <summary>
/// Response after requesting stop payment.
/// </summary>
public record CheckStopPaymentResponse(DefaultIdType Id, string CheckNumber, string Status);

