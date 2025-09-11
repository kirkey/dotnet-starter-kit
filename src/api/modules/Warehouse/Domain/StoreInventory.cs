using FSH.Framework.Core.Domain;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public class StoreInventory : AuditableEntity
{
    public int QuantityOnHand { get; private set; }
    public int QuantityReserved { get; private set; }
    public int QuantityAvailable { get; private set; }
    public DateTime LastUpdated { get; private set; }

    public DefaultIdType StoreId { get; private set; }
    public Store Store { get; private set; } = default!;
    public DefaultIdType ProductId { get; private set; }
    public Product Product { get; private set; } = default!;

    private StoreInventory() { }

    public static StoreInventory Create(DefaultIdType storeId, DefaultIdType productId, int quantityOnHand, int quantityReserved)
    {
        return new StoreInventory
        {
            StoreId = storeId,
            ProductId = productId,
            QuantityOnHand = quantityOnHand,
            QuantityReserved = quantityReserved,
            QuantityAvailable = quantityOnHand - quantityReserved,
            LastUpdated = DateTime.UtcNow
        };
    }

    public StoreInventory Update(int? quantityOnHand, int? quantityReserved)
    {
        if (quantityOnHand.HasValue) QuantityOnHand = quantityOnHand.Value;
        if (quantityReserved.HasValue) QuantityReserved = quantityReserved.Value;
        QuantityAvailable = QuantityOnHand - QuantityReserved;
        LastUpdated = DateTime.UtcNow;
        return this;
    }
}

