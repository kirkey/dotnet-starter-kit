namespace Store.Domain.Exceptions.GroceryItem;

/// <summary>
/// Thrown when provided stock level parameters are invalid or inconsistent.
/// </summary>
public sealed class InvalidGroceryItemStockLevelException(string message)
    : BadRequestException(message) {}

