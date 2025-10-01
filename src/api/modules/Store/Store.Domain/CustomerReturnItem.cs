namespace Store.Domain;

/// <summary>
/// Represents a line item in a customer return/refund.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track individual items and quantities being returned.
/// - Calculate line-level refund amounts.
/// - Support partial returns of multi-item purchases.
/// </remarks>
/// <seealso cref="Store.Domain.Events.CustomerReturnItemAdded"/>
/// <seealso cref="Store.Domain.Exceptions.CustomerReturn.CustomerReturnNotFoundException"/>
public sealed class CustomerReturnItem : AuditableEntity
{
    /// <summary>
    /// Parent customer return identifier.
    /// Example: a <see cref="CustomerReturn"/> Id.
    /// </summary>
    public DefaultIdType CustomerReturnId { get; private set; }

    /// <summary>
    /// Grocery item being returned.
    /// Example: an existing <see cref="GroceryItem"/> Id.
    /// </summary>
    public DefaultIdType GroceryItemId { get; private set; }

    /// <summary>
    /// Quantity being returned. Must be positive.
    /// Example: 2.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Unit price for refund calculation. Must be &gt;= 0.
    /// Example: 2.49.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    private CustomerReturnItem() { }

    private CustomerReturnItem(DefaultIdType id, DefaultIdType returnId, DefaultIdType groceryItemId, string name, int quantity, decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
        if (name.Length > 200) throw new ArgumentException("Name must not exceed 200 characters", nameof(name));
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (unitPrice < 0m) throw new ArgumentException("UnitPrice must be zero or greater", nameof(unitPrice));

        Id = id;
        CustomerReturnId = returnId;
        GroceryItemId = groceryItemId;
        Name = name;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    /// <summary>
    /// Factory to create a customer return line item.
    /// </summary>
    /// <param name="returnId">Parent return id.</param>
    /// <param name="groceryItemId">Item being returned.</param>
    /// <param name="name">Item name. Example: "Bananas".</param>
    /// <param name="quantity">Return quantity. Example: 2.</param>
    /// <param name="unitPrice">Refund price per unit. Example: 2.49.</param>
    public static CustomerReturnItem Create(DefaultIdType returnId, DefaultIdType groceryItemId, string name, int quantity, decimal unitPrice)
        => new(DefaultIdType.NewGuid(), returnId, groceryItemId, name, quantity, unitPrice);
}
