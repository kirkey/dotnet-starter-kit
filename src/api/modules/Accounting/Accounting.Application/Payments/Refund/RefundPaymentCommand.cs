namespace Accounting.Application.Payments.Refund;

/// <summary>
/// Command to refund a payment or partial payment amount.
/// </summary>
public sealed record RefundPaymentCommand(
    DefaultIdType PaymentId,
    decimal RefundAmount,
    string? RefundReference = null
) : IRequest<DefaultIdType>;
