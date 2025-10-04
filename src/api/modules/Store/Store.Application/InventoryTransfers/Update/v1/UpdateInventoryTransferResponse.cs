namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Update.v1;

/// <summary>
/// Response returned after updating an inventory transfer.
/// </summary>
/// <param name="Id">The identifier of the updated inventory transfer.</param>
public record UpdateInventoryTransferResponse(DefaultIdType Id);

