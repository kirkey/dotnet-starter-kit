namespace Accounting.Application.Accruals.Reject;

/// <summary>
/// Command to reject an accrual.
/// </summary>
public sealed record RejectAccrualCommand(
    DefaultIdType AccrualId,
    string RejectedBy,
    string? Reason
) : IRequest<DefaultIdType>;
