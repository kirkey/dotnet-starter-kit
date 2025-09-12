namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Get.v1;

public record GetInventoryTransferQuery(DefaultIdType Id) : IRequest<GetInventoryTransferResponse>;
