using FSH.Starter.WebApi.Store.Application.SerialNumbers.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Update.v1;

public class UpdateSerialNumberCommand : IRequest<UpdateSerialNumberResponse>
{
    public DefaultIdType Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = default!;
    public DefaultIdType? WarehouseId { get; set; }
    public DefaultIdType? WarehouseLocationId { get; set; }
    public DefaultIdType? BinId { get; set; }
    public DefaultIdType? LotNumberId { get; set; }
    public DateTime? ManufactureDate { get; set; }
    public DateTime? WarrantyExpirationDate { get; set; }
    public string? ExternalReference { get; set; }
    public string? Notes { get; set; }
}

public class UpdateSerialNumberResponse(SerialNumberResponse serialNumber)
{
    public SerialNumberResponse SerialNumber { get; } = serialNumber;
}
