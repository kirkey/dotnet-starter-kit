namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Create.v1;

/// <summary>
/// Response after creating a fee payment.
/// </summary>
public sealed record CreateFeePaymentResponse(
    DefaultIdType Id,
    string Reference,
    decimal Amount,
    string Status);
