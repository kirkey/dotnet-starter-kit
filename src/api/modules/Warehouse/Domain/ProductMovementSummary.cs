using FSH.Framework.Core.Domain;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public class ProductMovementSummary : AuditableEntity
{
    public DefaultIdType ProductId { get; private set; }
    public Product Product { get; private set; } = default!;
    public DateTime PeriodStart { get; private set; }
    public DateTime PeriodEnd { get; private set; }
    public int TotalPurchased { get; private set; }
    public int TotalSold { get; private set; }
    public int DaysInPeriod { get; private set; }
    public decimal AverageDailySales { get; private set; }
    public decimal TurnoverRate { get; private set; }
    public int DaysFromPurchaseToSale { get; private set; }

    public DefaultIdType? StoreId { get; private set; }
    public Store? Store { get; private set; }
    public DefaultIdType? WarehouseId { get; private set; }
    public Warehouse? Warehouse { get; private set; }

    private ProductMovementSummary() { }
}

