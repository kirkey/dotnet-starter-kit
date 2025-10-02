namespace Accounting.Application.Payments.Void;

/// <summary>
/// Command to void a payment (reverses entire payment transaction).
/// </summary>
public sealed record VoidPaymentCommand(
    DefaultIdType PaymentId,
    string? VoidReason = null
) : IRequest<DefaultIdType>;
