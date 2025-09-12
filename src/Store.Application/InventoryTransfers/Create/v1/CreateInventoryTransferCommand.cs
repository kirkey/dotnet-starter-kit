namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Create.v1;

public record CreateInventoryTransferCommand(
    [property: DefaultValue("TRF001")] string TransferNumber,
    DefaultIdType FromWarehouseId,
    DefaultIdType ToWarehouseId,
    [property: DefaultValue(null)] DefaultIdType? FromLocationId,
    [property: DefaultValue(null)] DefaultIdType? ToLocationId,
    [property: DefaultValue("2024-01-01")] DateTime TransferDate,
    [property: DefaultValue(null)] DateTime? ExpectedArrivalDate,
    [property: DefaultValue("Standard")] string TransferType,
    [property: DefaultValue("Normal")] string Priority,
    [property: DefaultValue(null)] string? TransportMethod,
    [property: DefaultValue(null)] string? Notes,
    [property: DefaultValue(null)] string? RequestedBy) : IRequest<CreateInventoryTransferResponse>;
