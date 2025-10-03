namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Create.v1;

public class CreateSerialNumberCommand : IRequest<CreateSerialNumberResponse>
{
    public string SerialNumberValue { get; set; } = default!;
    public DefaultIdType ItemId { get; set; }
    public DefaultIdType? WarehouseId { get; set; }
    public DefaultIdType? WarehouseLocationId { get; set; }
    public DefaultIdType? BinId { get; set; }
    public DefaultIdType? LotNumberId { get; set; }
    public DateTime? ReceiptDate { get; set; }
    public DateTime? ManufactureDate { get; set; }
    public DateTime? WarrantyExpirationDate { get; set; }
    public string? ExternalReference { get; set; }
    public string? Notes { get; set; }
}

public class CreateSerialNumberResponse(DefaultIdType id, string serialNumberValue)
{
    public DefaultIdType Id { get; } = id;
    public string SerialNumberValue { get; } = serialNumberValue;
}
