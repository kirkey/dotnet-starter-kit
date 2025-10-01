namespace Store.Domain.Exceptions.GroceryItem;

public sealed class GroceryItemNotFoundException(DefaultIdType id)
    : NotFoundException($"Grocery Item with ID '{id}' was not found.") {}

