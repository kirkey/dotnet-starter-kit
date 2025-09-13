using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Store.Domain.Events;

namespace Store.Domain;

public sealed class CycleCountItem : AuditableEntity, IAggregateRoot
{
    public DefaultIdType CycleCountId { get; private set; }
    public DefaultIdType GroceryItemId { get; private set; }
    public int SystemQuantity { get; private set; }
    public int? CountedQuantity { get; private set; }
    public int? VarianceQuantity { get; private set; }
    
    public DateTime? CountDate { get; private set; }
    public string? CountedBy { get; private set; }
    public bool RequiresRecount { get; private set; }
    public string? RecountReason { get; private set; }
    
    public CycleCount CycleCount { get; private set; } = default!;
    public GroceryItem GroceryItem { get; private set; } = default!;

    private CycleCountItem() { }

    private CycleCountItem(
        DefaultIdType id,
        DefaultIdType cycleCountId,
        DefaultIdType groceryItemId,
        int systemQuantity,
        int? countedQuantity,
        string? notes)
    {
        Id = id;
        CycleCountId = cycleCountId;
        GroceryItemId = groceryItemId;
        SystemQuantity = systemQuantity;
        CountedQuantity = countedQuantity;
        Notes = notes;
        RequiresRecount = false;
        
        if (countedQuantity.HasValue)
        {
            CalculateVariance();
            CountDate = DateTime.UtcNow;
        }

        QueueDomainEvent(new CycleCountItemCreated { CycleCountItem = this });
    }

    public static CycleCountItem Create(
        DefaultIdType cycleCountId,
        DefaultIdType groceryItemId,
        int systemQuantity,
        int? countedQuantity = null,
        string? notes = null)
    {
        return new CycleCountItem(
            DefaultIdType.NewGuid(),
            cycleCountId,
            groceryItemId,
            systemQuantity,
            countedQuantity,
            notes);
    }

    public CycleCountItem RecordCount(int countedQuantity, string? countedBy = null)
    {
        CountedQuantity = countedQuantity;
        CountedBy = countedBy;
        CountDate = DateTime.UtcNow;
        CalculateVariance();
        
        QueueDomainEvent(new CycleCountItemCounted { CycleCountItem = this });
        return this;
    }

    public CycleCountItem MarkForRecount(string reason)
    {
        RequiresRecount = true;
        RecountReason = reason;
        QueueDomainEvent(new CycleCountItemMarkedForRecount { CycleCountItem = this, Reason = reason });
        return this;
    }

    private void CalculateVariance()
    {
        if (CountedQuantity.HasValue)
        {
            VarianceQuantity = CountedQuantity.Value - SystemQuantity;
        }
    }

    public bool IsAccurate() => VarianceQuantity == 0;
    public bool HasPositiveVariance() => VarianceQuantity > 0;
    public bool HasNegativeVariance() => VarianceQuantity < 0;
    public bool HasSignificantVariance(int threshold = 5) => Math.Abs(VarianceQuantity ?? 0) > threshold;
    public bool IsCounted() => CountedQuantity.HasValue;
}
