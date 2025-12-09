namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Reverse.v1;

/// <summary>
/// Response after reversing a fee payment.
/// </summary>
public sealed record ReverseFeePaymentResponse(DefaultIdType Id, string Status);
