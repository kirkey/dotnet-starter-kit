namespace Accounting.Application.PaymentAllocations.Commands;

/// <summary>
/// Command to create a new payment allocation.
/// Allocates a payment amount to a specific invoice.
/// </summary>
/// <remarks>
/// Payment Allocation Creation:
/// - PaymentId: The payment being allocated (required)
/// - InvoiceId: The invoice receiving the allocation (required)
/// - Amount: The amount to allocate (required, must be positive)
/// - Notes: Optional notes about the allocation
/// 
/// Business Rules:
/// - Amount must be positive
/// - Cannot allocate more than the payment's unapplied amount
/// - Cannot allocate to a fully paid invoice
/// - Payment and Invoice must exist
/// </remarks>
public sealed record CreatePaymentAllocationCommand(
    DefaultIdType PaymentId,
    DefaultIdType InvoiceId,
    decimal Amount,
    string? Notes = null
) : IRequest<DefaultIdType>;

