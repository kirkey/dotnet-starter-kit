using FSH.Framework.Core.Domain;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public class WarehouseInventory : AuditableEntity
{
    public int QuantityOnHand { get; private set; }
    public int QuantityReserved { get; private set; }
    public int QuantityAvailable { get; private set; }
    public DateTime LastUpdated { get; private set; }

    public DefaultIdType WarehouseId { get; private set; }
    public Warehouse Warehouse { get; private set; } = default!;
    public DefaultIdType ProductId { get; private set; }
    public Product Product { get; private set; } = default!;

    private WarehouseInventory() { }

    public static WarehouseInventory Create(DefaultIdType warehouseId, DefaultIdType productId, int quantityOnHand, int quantityReserved)
    {
        return new WarehouseInventory
        {
            WarehouseId = warehouseId,
            ProductId = productId,
            QuantityOnHand = quantityOnHand,
            QuantityReserved = quantityReserved,
            QuantityAvailable = quantityOnHand - quantityReserved,
            LastUpdated = DateTime.UtcNow
        };
    }

    public WarehouseInventory Update(int? quantityOnHand, int? quantityReserved)
    {
        if (quantityOnHand.HasValue) QuantityOnHand = quantityOnHand.Value;
        if (quantityReserved.HasValue) QuantityReserved = quantityReserved.Value;
        QuantityAvailable = QuantityOnHand - QuantityReserved;
        LastUpdated = DateTime.UtcNow;
        return this;
    }
}

