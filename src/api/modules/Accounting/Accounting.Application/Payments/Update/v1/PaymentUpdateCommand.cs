namespace Accounting.Application.Payments.Update.v1;

/// <summary>
/// Command to update an existing payment.
/// </summary>
/// <remarks>
/// Only updates basic payment information.
/// Cannot update Amount or Allocations through this command.
/// Use AllocatePayment, RefundPayment, or VoidPayment for those operations.
/// </remarks>
public sealed record PaymentUpdateCommand(
    DefaultIdType Id,
    string? ReferenceNumber,
    string? DepositToAccountCode,
    string? Description,
    string? Notes
) : IRequest<PaymentUpdateResponse>;

