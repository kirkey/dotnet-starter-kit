namespace Accounting.Application.Payments.Create.v1;

/// <summary>
/// Command to create a new payment record.
/// Represents money received from customers/members.
/// </summary>
/// <remarks>
/// Payment Creation:
/// - PaymentNumber: Unique receipt/payment number for tracking
/// - MemberId: Optional member/customer identifier
/// - PaymentDate: Date when payment was received
/// - Amount: Total payment amount (must be positive)
/// - PaymentMethod: Cash, Check, EFT, CreditCard, etc.
/// - ReferenceNumber: Check number or transaction reference
/// - DepositToAccountCode: Bank/cash account where payment is deposited
/// 
/// Business Rules:
/// - Payment amount must be positive
/// - Initially, UnappliedAmount equals Amount (no allocations yet)
/// - Use AllocatePayment command separately to apply to invoices
/// - PaymentNumber must be unique
/// </remarks>
public sealed record CreatePaymentCommand(
    string PaymentNumber,
    DefaultIdType? MemberId,
    DateTime PaymentDate,
    decimal Amount,
    string PaymentMethod,
    string? ReferenceNumber,
    string? DepositToAccountCode,
    string? Description,
    string? Notes
) : IRequest<PaymentCreateResponse>;

