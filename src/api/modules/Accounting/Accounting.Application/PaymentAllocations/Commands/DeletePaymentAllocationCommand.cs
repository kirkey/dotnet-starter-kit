namespace Accounting.Application.PaymentAllocations.Commands;

/// <summary>
/// Command to delete a payment allocation.
/// </summary>
/// <remarks>
/// Business Rules:
/// - Cannot delete allocation if invoice is already marked as paid
/// - Deleting allocation returns funds to payment's unapplied amount
/// - Consider business impact before deletion (audit trail)
/// </remarks>
public sealed record DeletePaymentAllocationCommand(DefaultIdType Id) : IRequest<Unit>;
