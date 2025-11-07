namespace Accounting.Application.PaymentAllocations.Commands;

/// <summary>
/// Command to update an existing payment allocation.
/// </summary>
/// <remarks>
/// Update Rules:
/// - Can update the allocation amount and/or notes
/// - If Amount is provided, it must be positive
/// - At least one field (Amount or Notes) must be provided
/// - Cannot change PaymentId or InvoiceId (delete and recreate instead)
/// 
/// Business Rules:
/// - Amount must remain positive if updated
/// - Cannot exceed payment's available amount
/// - May affect invoice payment status
/// </remarks>
public sealed record UpdatePaymentAllocationCommand(
    DefaultIdType Id,
    decimal? Amount = null,
    string? Notes = null
) : IRequest<DefaultIdType>;
