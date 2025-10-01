namespace Store.Domain;

/// <summary>
/// Represents a single counted line in a cycle count (item-level result).
/// Stores system vs counted quantities and any variance information.
/// </summary>
/// <remarks>
/// Use cases:
/// - Record counted quantities during a physical count.
/// - Flag items for recount when variance is large.
/// </remarks>
public sealed class CycleCountItem : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// The parent cycle count identifier.
    /// </summary>
    public DefaultIdType CycleCountId { get; private set; }

    /// <summary>
    /// The grocery item being counted.
    /// </summary>
    public DefaultIdType GroceryItemId { get; private set; }

    /// <summary>
    /// Quantity recorded in the system before counting. Example: 10.
    /// </summary>
    public int SystemQuantity { get; private set; }

    /// <summary>
    /// Quantity observed during the count. Null until counted.
    /// </summary>
    public int? CountedQuantity { get; private set; }

    /// <summary>
    /// Difference between counted and system quantity (Counted - System).
    /// </summary>
    public int? VarianceQuantity { get; private set; }
    
    /// <summary>
    /// Date when the item was counted.
    /// </summary>
    public DateTime? CountDate { get; private set; }

    /// <summary>
    /// Identifier of the user who counted the item.
    /// </summary>
    public string? CountedBy { get; private set; }

    /// <summary>
    /// Indicates if the item requires recounting due to variance.
    /// </summary>
    public bool RequiresRecount { get; private set; }

    /// <summary>
    /// Reason for recounting the item, if applicable.
    /// </summary>
    public string? RecountReason { get; private set; }
    
    /// <summary>
    /// The parent cycle count aggregate.
    /// </summary>
    public CycleCount CycleCount { get; private set; } = default!;

    /// <summary>
    /// The grocery item details.
    /// </summary>
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
        // validations
        if (cycleCountId == default) throw new ArgumentException("CycleCountId is required", nameof(cycleCountId));
        if (groceryItemId == default) throw new ArgumentException("GroceryItemId is required", nameof(groceryItemId));
        if (systemQuantity < 0) throw new ArgumentException("SystemQuantity must be zero or greater", nameof(systemQuantity));
        if (countedQuantity.HasValue && countedQuantity.Value < 0) throw new ArgumentException("CountedQuantity must be zero or greater when provided", nameof(countedQuantity));

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

    /// <summary>
    /// Factory method to create a new CycleCountItem.
    /// </summary>
    /// <param name="cycleCountId">The cycle count identifier.</param>
    /// <param name="groceryItemId">The grocery item identifier.</param>
    /// <param name="systemQuantity">The system quantity of the item.</param>
    /// <param name="countedQuantity">The counted quantity of the item (optional).</param>
    /// <param name="notes">Additional notes about the item (optional).</param>
    /// <returns>A new instance of CycleCountItem.</returns>
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

    /// <summary>
    /// Records the counted quantity for the item.
    /// </summary>
    /// <param name="countedQuantity">The quantity counted.</param>
    /// <param name="countedBy">The identifier of the user who counted (optional).</param>
    /// <returns>The updated CycleCountItem.</returns>
    public CycleCountItem RecordCount(int countedQuantity, string? countedBy = null)
    {
        CountedQuantity = countedQuantity;
        CountedBy = countedBy;
        CountDate = DateTime.UtcNow;
        CalculateVariance();
        
        QueueDomainEvent(new CycleCountItemCounted { CycleCountItem = this });
        return this;
    }

    /// <summary>
    /// Marks the item for recounting due to significant variance.
    /// </summary>
    /// <param name="reason">The reason for the recount.</param>
    /// <returns>The updated CycleCountItem.</returns>
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

    /// <summary>
    /// Checks if the counted quantity matches the system quantity.
    /// </summary>
    /// <returns>True if accurate, false otherwise.</returns>
    public bool IsAccurate() => VarianceQuantity == 0;

    /// <summary>
    /// Checks if there is a positive variance (i.e., counted quantity is greater).
    /// </summary>
    /// <returns>True if positive variance, false otherwise.</returns>
    public bool HasPositiveVariance() => VarianceQuantity > 0;

    /// <summary>
    /// Checks if there is a negative variance (i.e., counted quantity is lesser).
    /// </summary>
    /// <returns>True if negative variance, false otherwise.</returns>
    public bool HasNegativeVariance() => VarianceQuantity < 0;

    /// <summary>
    /// Checks if the variance is significant based on a threshold.
    /// </summary>
    /// <param name="threshold">The variance threshold (default is 5).</param>
    /// <returns>True if variance is significant, false otherwise.</returns>
    public bool HasSignificantVariance(int threshold = 5) => Math.Abs(VarianceQuantity ?? 0) > threshold;

    /// <summary>
    /// Checks if the item has been counted (i.e., counted quantity is not null).
    /// </summary>
    /// <returns>True if counted, false otherwise.</returns>
    public bool IsCounted() => CountedQuantity.HasValue;
}
