using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Reject.v1;

/// <summary>
/// Command to reject an inventory transaction.
/// </summary>
public class RejectInventoryTransactionCommand : IRequest<RejectInventoryTransactionResponse>
{
    /// <summary>
    /// Transaction ID to reject.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// User who rejected the transaction.
    /// </summary>
    public string RejectedBy { get; set; } = default!;

    /// <summary>
    /// Reason for rejection.
    /// </summary>
    public string? RejectionReason { get; set; }
}

/// <summary>
/// Response after rejecting an inventory transaction.
/// </summary>
public class RejectInventoryTransactionResponse(InventoryTransactionResponse transaction)
{
    /// <summary>
    /// The rejected transaction.
    /// </summary>
    public InventoryTransactionResponse Transaction { get; } = transaction;
}

