namespace Store.Domain.Exceptions.ItemSupplier;

/// <summary>
/// Exception thrown when an item-supplier relationship is not found.
/// </summary>
public class ItemSupplierNotFoundException : NotFoundException
{
    public ItemSupplierNotFoundException(DefaultIdType itemSupplierId)
        : base($"Item-supplier relationship with ID '{itemSupplierId}' was not found.")
    {
    }

    public ItemSupplierNotFoundException(DefaultIdType itemId, DefaultIdType supplierId)
        : base($"Item-supplier relationship for item '{itemId}' and supplier '{supplierId}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when attempting to create a duplicate item-supplier relationship.
/// </summary>
public class DuplicateItemSupplierException : ConflictException
{
    public DuplicateItemSupplierException(DefaultIdType itemId, DefaultIdType supplierId)
        : base($"Item-supplier relationship for item '{itemId}' and supplier '{supplierId}' already exists.")
    {
    }
}
