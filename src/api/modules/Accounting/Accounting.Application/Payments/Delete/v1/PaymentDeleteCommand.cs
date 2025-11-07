namespace Accounting.Application.Payments.Delete.v1;

/// <summary>
/// Command to delete a payment.
/// </summary>
/// <remarks>
/// Business Rules:
/// - Cannot delete payment if it has allocations
/// - Consider using Void instead of Delete for audit trail
/// </remarks>
public sealed record PaymentDeleteCommand(DefaultIdType Id) : IRequest;
