namespace Store.Domain.Exceptions.Items;

/// <summary>
/// Exception thrown when an item is not found.
/// </summary>
public class ItemNotFoundException : NotFoundException
{
    public ItemNotFoundException(DefaultIdType itemId)
        : base($"Item with ID '{itemId}' was not found.")
    {
    }

    public ItemNotFoundException(string sku)
        : base($"Item with SKU '{sku}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when attempting to create an item with a duplicate SKU.
/// </summary>
public class DuplicateItemSkuException : ConflictException
{
    public DuplicateItemSkuException(string sku)
        : base($"An item with SKU '{sku}' already exists.")
    {
    }
}

/// <summary>
/// Exception thrown when attempting to create an item with a duplicate barcode.
/// </summary>
public class DuplicateItemBarcodeException : ConflictException
{
    public DuplicateItemBarcodeException(string barcode)
        : base($"An item with barcode '{barcode}' already exists.")
    {
    }
}

/// <summary>
/// Exception thrown when item stock level validation fails.
/// </summary>
public class InvalidItemStockLevelException(string message) : BadRequestException(message);

/// <summary>
/// Exception thrown when an item cannot be deleted.
/// </summary>
public class ItemCannotBeDeletedException(DefaultIdType itemId, string reason) 
    : BadRequestException($"Item with ID '{itemId}' cannot be deleted: {reason}");
