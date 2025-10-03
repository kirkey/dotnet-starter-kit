namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Get.v1;

/// <summary>
/// Command to get an inventory transaction by ID.
/// </summary>
public sealed record GetInventoryTransactionCommand(DefaultIdType Id) : IRequest<InventoryTransactionResponse>;
