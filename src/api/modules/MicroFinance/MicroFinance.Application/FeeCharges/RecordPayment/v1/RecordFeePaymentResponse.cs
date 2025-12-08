namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.RecordPayment.v1;

/// <summary>
/// Response after recording payment.
/// </summary>
public sealed record RecordFeePaymentResponse(
    DefaultIdType FeeChargeId,
    decimal AmountPaid,
    decimal Outstanding,
    string Status,
    string Message);
