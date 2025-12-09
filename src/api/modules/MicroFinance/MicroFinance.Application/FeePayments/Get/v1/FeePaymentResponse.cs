namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Get.v1;

/// <summary>
/// Response containing fee payment details.
/// </summary>
public sealed record FeePaymentResponse(
    DefaultIdType Id,
    DefaultIdType FeeChargeId,
    DefaultIdType? LoanRepaymentId,
    DefaultIdType? SavingsTransactionId,
    string Reference,
    DateOnly PaymentDate,
    decimal Amount,
    string PaymentMethod,
    string PaymentSource,
    string Status,
    string? ReversalReason,
    DateOnly? ReversedDate,
    string? Notes);
