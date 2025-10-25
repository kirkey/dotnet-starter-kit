using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.UpdateNotes.v1;

/// <summary>
/// Command to update notes on an inventory transaction.
/// </summary>
public class UpdateInventoryTransactionNotesCommand : IRequest<UpdateInventoryTransactionNotesResponse>
{
    /// <summary>
    /// Transaction ID to update.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Updated notes content.
    /// </summary>
    public string? Notes { get; set; }
}

/// <summary>
/// Response after updating inventory transaction notes.
/// </summary>
public class UpdateInventoryTransactionNotesResponse(InventoryTransactionResponse transaction)
{
    /// <summary>
    /// The updated transaction.
    /// </summary>
    public InventoryTransactionResponse Transaction { get; } = transaction;
}

