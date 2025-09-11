using FSH.Framework.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public class ExpiryAlert : AuditableEntity
{
    public DefaultIdType ProductBatchId { get; private set; }
    public ProductBatch ProductBatch { get; private set; } = default!;
    public DateTime ExpiryDate { get; private set; }
    public int DaysUntilExpiry { get; private set; }
    public int CurrentQuantity { get; private set; }
    [MaxLength(100)]
    public string AlertLevel { get; private set; } = string.Empty;
    public DateTime AlertDate { get; private set; }
    public bool IsResolved { get; private set; }
    [MaxLength(100)]
    public string? ActionTaken { get; private set; }

    public DefaultIdType? StoreId { get; private set; }
    public Store? Store { get; private set; }
    public DefaultIdType? WarehouseId { get; private set; }
    public Warehouse? Warehouse { get; private set; }

    private ExpiryAlert() { }

    public static ExpiryAlert Create(
        DefaultIdType productBatchId,
        DateTime expiryDate,
        int daysUntilExpiry,
        int currentQuantity,
        string alertLevel,
        DateTime alertDate,
        bool isResolved,
        string? actionTaken,
        DefaultIdType? storeId,
        DefaultIdType? warehouseId)
    {
        return new ExpiryAlert
        {
            ProductBatchId = productBatchId,
            ExpiryDate = expiryDate,
            DaysUntilExpiry = daysUntilExpiry,
            CurrentQuantity = currentQuantity,
            AlertLevel = alertLevel,
            AlertDate = alertDate,
            IsResolved = isResolved,
            ActionTaken = actionTaken,
            StoreId = storeId,
            WarehouseId = warehouseId
        };
    }

    public ExpiryAlert Update(
        DateTime? expiryDate,
        int? daysUntilExpiry,
        int? currentQuantity,
        string? alertLevel,
        DateTime? alertDate,
        bool? isResolved,
        string? actionTaken)
    {
        if (expiryDate.HasValue) ExpiryDate = expiryDate.Value;
        if (daysUntilExpiry.HasValue) DaysUntilExpiry = daysUntilExpiry.Value;
        if (currentQuantity.HasValue) CurrentQuantity = currentQuantity.Value;
        if (alertLevel is not null) AlertLevel = alertLevel;
        if (alertDate.HasValue) AlertDate = alertDate.Value;
        if (isResolved.HasValue) IsResolved = isResolved.Value;
        if (actionTaken is not null) ActionTaken = actionTaken;
        return this;
    }
}
