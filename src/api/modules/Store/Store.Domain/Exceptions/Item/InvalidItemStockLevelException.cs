namespace Store.Domain.Exceptions.Item;

/// <summary>
/// Thrown when provided stock level parameters are invalid or inconsistent.
/// </summary>
public sealed class InvalidItemStockLevelException(string message)
    : BadRequestException(message) {}

