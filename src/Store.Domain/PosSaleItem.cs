namespace Store.Domain;

/// <summary>
/// Represents a single line in a POS sale with pricing and quantity details.
/// </summary>
/// <remarks>
/// Use cases:
/// - Capture scanned/entered items during checkout.
/// - Compute per-line totals and taxes for receipt printing.
/// - Support line-level discounts and itemized taxes.
/// </remarks>
/// <seealso cref="Store.Domain.Events.PosSaleItemAdded"/>
/// <seealso cref="Store.Domain.Events.PosSaleCreated"/>
/// <seealso cref="Store.Domain.Events.PosSaleCompleted"/>
/// <seealso cref="Store.Domain.Events.PosSaleVoided"/>
/// <seealso cref="Store.Domain.Exceptions.PosSale.PosSaleNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.PosSale.PosSaleInvalidOperationException"/>
public sealed class PosSaleItem : AuditableEntity
{
    /// <summary>
    /// The parent POS sale identifier this item belongs to.
    /// Example: 00000000-0000-0000-0000-000000000001.
    /// </summary>
    public DefaultIdType PosSaleId { get; private set; }

    /// <summary>
    /// The referenced grocery item identifier.
    /// Example: an existing <see cref="GroceryItem"/> Id.
    /// </summary>
    public DefaultIdType GroceryItemId { get; private set; }

    /// <summary>
    /// Unit price used for this line.
    /// Example: 2.49. Default must be &gt;= 0.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Quantity sold for this line.
    /// Example: 3. Must be &gt; 0.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Discount amount applied to this line.
    /// Example: 0.50. Default: 0.00.
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Tax amount applied to this line.
    /// Example: 0.45. Default: 0.00.
    /// </summary>
    public decimal Tax { get; private set; }

    private PosSaleItem() { }

    private PosSaleItem(DefaultIdType id, DefaultIdType posSaleId, DefaultIdType groceryItemId, string name, decimal unitPrice, int quantity, decimal discount, decimal tax)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
        if (name.Length > 200) throw new ArgumentException("Name must not exceed 200 characters", nameof(name));
        if (unitPrice < 0m) throw new ArgumentException("Unit price must be zero or greater", nameof(unitPrice));
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (discount < 0m) throw new ArgumentException("Discount cannot be negative", nameof(discount));
        if (tax < 0m) throw new ArgumentException("Tax cannot be negative", nameof(tax));

        Id = id;
        PosSaleId = posSaleId;
        GroceryItemId = groceryItemId;
        Name = name;
        UnitPrice = unitPrice;
        Quantity = quantity;
        Discount = discount;
        Tax = tax;
    }

    /// <summary>
    /// Factory to create a POS sale line item for a given sale and grocery item.
    /// </summary>
    /// <param name="posSaleId">Parent POS sale id. Example: a <see cref="PosSale"/> Id.</param>
    /// <param name="groceryItemId">Grocery item id being sold.</param>
    /// <param name="name">Snapshot of item name. Example: "Bananas".</param>
    /// <param name="unitPrice">Unit price used. Example: 2.49 (must be &gt;= 0).</param>
    /// <param name="quantity">Quantity sold. Example: 3 (must be &gt; 0).</param>
    /// <param name="discount">Line discount amount. Example: 0.50 (default 0.00).</param>
    /// <param name="tax">Line tax amount. Example: 0.45 (default 0.00).</param>
    /// <returns>New <see cref="PosSaleItem"/> instance.</returns>
    public static PosSaleItem Create(DefaultIdType posSaleId, DefaultIdType groceryItemId, string name, decimal unitPrice, int quantity, decimal discount, decimal tax)
        => new(DefaultIdType.NewGuid(), posSaleId, groceryItemId, name, unitPrice, quantity, discount, tax);
}
